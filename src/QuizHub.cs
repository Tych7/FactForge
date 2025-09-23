using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Microsoft.AspNetCore.SignalR;
using QRCoder;
using SkiaSharp;

namespace DesktopApp;

public class QuizHub : Hub
{
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        throw new Exception("No IPv4 address found");
    }

    public Bitmap GenerateQrCode(string url, int pixelsPerModule = 20)
    {
        // 1. Generate QR code data (matrix of true/false)
        using var qrGen = new QRCodeGenerator();
        QRCodeData qrData = qrGen.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

        int modules = qrData.ModuleMatrix.Count;
        int size = modules * pixelsPerModule;

        // 2. Create an SKBitmap to draw on
        using var skBitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(skBitmap);

        // 3. Fill background white
        canvas.Clear(SKColors.White);

        // 4. Draw black squares for "true" modules
        using var paint = new SKPaint { Color = SKColors.Black, IsAntialias = false, Style = SKPaintStyle.Fill };

        for (int y = 0; y < modules; y++)
        {
            for (int x = 0; x < modules; x++)
            {
                if (qrData.ModuleMatrix[y][x])
                {
                    var rect = new SKRect(
                        x * pixelsPerModule,
                        y * pixelsPerModule,
                        (x + 1) * pixelsPerModule,
                        (y + 1) * pixelsPerModule
                    );
                    canvas.DrawRect(rect, paint);
                }
            }
        }

        // 5. Convert SKBitmap → Avalonia Bitmap
        using var ms = new MemoryStream();
        skBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
        ms.Position = 0;
        return new Bitmap(ms);
    }


    public async Task SendQuestion(string questionText, string[] choices)
    {
        await Clients.All.SendAsync("NewQuestion", new
        {
            text = questionText,
            choices = choices
        });
    }

    public async Task SubmitAnswer(string name, string answer)
    {
        await Clients.All.SendAsync("AnswerReceived", name, answer);
        Console.WriteLine($"{name}: {answer}");
    }

    public Task JoinGame(string name)
    {
        Console.WriteLine($"{Context.ConnectionId} joined as {name}");
        return Task.CompletedTask;
    }


}
