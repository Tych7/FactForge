using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Tmds.DBus.Protocol;
using static QuizSlide;

namespace DesktopApp;

public static class QuizDataHandler
{
    static QuizDataHandler(){}

    public static readonly string SlideImageStoragePath = Path.Combine(Environment.CurrentDirectory, "Data", "SlideImages");

    private static readonly Dictionary<string, Bitmap> _bitmapCache = new();

    private static QuizSlide defaultQuestionSlide = new QuizSlide
    {
        Id = 0,
        Type = SlideTypes.MultipleChoiceQuestion,
        Question = string.Empty,
        Answers = [string.Empty, string.Empty, string.Empty, string.Empty],
        CorrectAnswer = string.Empty,
        Time = 15,
        BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
        Category = string.Empty,
        ImagePath = string.Empty,
        AudioPath = string.Empty
    };

    private static QuizSlide defaultTextSlide = new QuizSlide
    {
        Id = 0,
        Type = SlideTypes.Text,
        Header = new QuizSlideText{Text = string.Empty, FontSize = 150},
        SubText = new QuizSlideText{Text = string.Empty, FontSize = 80},
        BgImagePath = "avares://DesktopApp/Assets/Backgrounds/BricksDesktop.png",
        Category = string.Empty,
        ImagePath = string.Empty,
        AudioPath = string.Empty
    };

    public static void CreateQuiz(string title, List<QuizSlide>? questions = null)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            Directory.CreateDirectory(quizPath);

        var quizData = new QuizData
        {
            Id = GetFirstAvailibleQuizId(),
            Favorite = false,
            Title = title,
            Quiz = questions ?? new List<QuizSlide>(),
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var filePath = Path.Combine(quizPath, $"quiz_{GetFirstAvailibleQuizId()}.json");

        var options = new JsonSerializerOptions { WriteIndented = true };

        var json = JsonSerializer.Serialize(quizData, options);
        File.WriteAllText(filePath, json);

        Console.WriteLine($"Quiz '{title}' created at {filePath}");
    }

    
   public static bool DeleteQuiz(string title)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            return false;

        var filePath = Path.Combine(quizPath, GetFileNameByTitle(title));

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{title}' not found at {filePath}");
            return false;
        }

        try
        {
            File.Delete(filePath);
            Console.WriteLine($"Quiz '{title}' deleted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting quiz '{title}': {ex.Message}");
            return false;
        }
    }

    public static (bool result, int index) CreateNewSlide(string quizTitle, SlideTypes type)
    {
        var filePath = GetFileNameByTitle(quizTitle);
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return (false , 0);

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);
            var slide = new QuizSlide();
            if(type == SlideTypes.Text) slide = defaultTextSlide;
            else
            {
                slide = defaultQuestionSlide;
                slide.Type = type;
            }
            
            if (quizData == null)
                return (false, 0);

            // Assign next available question ID
            if(quizData.Quiz != null)
            {
                int nextId = quizData.Quiz.Count == 0 ? 0 : quizData.Quiz.Max(q => q.Id) + 1;
                slide.Id = nextId;

                quizData.Quiz.Add(slide);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var updatedJson = JsonSerializer.Serialize(quizData, options);

                File.WriteAllText(filePath, updatedJson);

                Console.WriteLine($"Question added to quiz '{quizTitle}'.");
                return (true, nextId);
            }
            else
            {
                throw new Exception();
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding question: {ex.Message}");
            return (false, 0);
        }
    }

    public static bool InsertSlideAtIndex(string quizTitle, SlideTypes type, int index)
    {
        var filePath = GetFileNameByTitle(quizTitle);
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return false;

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);

            if (quizData == null)
                return false;

            // Create the new slide
            QuizSlide slide;
            if (type == SlideTypes.Text) slide = defaultTextSlide;
            else
            {
                slide = defaultQuestionSlide;
                slide.Type = type;
            }

            // Insert slide at specified index
            if (index < 0) index = 0;

            if(quizData.Quiz != null)
            {
                if (index > quizData.Quiz.Count) index = quizData.Quiz.Count;
                quizData.Quiz.Insert(index, slide);

                // Save temporarily to file so ReassignSlideIds can work
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(filePath, JsonSerializer.Serialize(quizData, options));

                // Reassign IDs to ensure sequential numbering
                return ReassignSlideIds(quizTitle);
            }
            else
            {
                throw new Exception();
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting slide: {ex.Message}");
            return false;
        }
    }


    public static bool DeleteSlide(string quizTitle, int questionId)
    {
        var filePath = GetFileNameByTitle(quizTitle);
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return false;

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);

            if (quizData == null || quizData.Quiz == null)
                return false;

            var questionToRemove = quizData.Quiz.FirstOrDefault(q => q.Id == questionId);

            if (questionToRemove == null)
            {
                Console.WriteLine($"Question with ID {questionId} not found in quiz '{quizTitle}'.");
                return false;
            }

            quizData.Quiz.Remove(questionToRemove);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(quizData, options);
            File.WriteAllText(filePath, updatedJson);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting question: {ex.Message}");
            return false;
        }
    }

    public static bool ReplaceQuizData(string currentQuizTitle, QuizData newQuizData)
    {
        if(newQuizData.Title != null)
        {
            DeleteQuiz(currentQuizTitle);
            CreateQuiz(newQuizData.Title, newQuizData.Quiz);
            return true;
        }
        else
        {
            Console.WriteLine("Title of replacement quizdata was null");
            return false;
        }
        
    }

    public static bool UpdateSlide(string quizTitle, QuizSlide updatedSlide)
    {
        var filePath = GetFileNameByTitle(quizTitle);

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{quizTitle}' not found.");
            return false;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);

            if (quizData == null || quizData.Quiz == null)
                return false;

            var index = quizData.Quiz.FindIndex(q => q.Id == updatedSlide.Id);

            if (index == -1)
            {
                Console.WriteLine($"Slide with ID {updatedSlide.Id} not found in quiz '{quizTitle}'.");
                return false;
            }

            quizData.Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            // Replace slide data
            quizData.Quiz[index] = updatedSlide;

            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(quizData, options);
            File.WriteAllText(filePath, updatedJson);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating slide: {ex.Message}");
            return false;
        }
    }

    public static bool ReassignSlideIds(string quizTitle)
    {
        var filePath = GetFileNameByTitle(quizTitle);

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{quizTitle}' not found.");
            return false;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            var quizData = JsonSerializer.Deserialize<QuizData>(json);

            if (quizData == null || quizData.Quiz == null || quizData.Quiz.Count == 0)
                return false;

            // Reassign IDs sequentially starting from 1
            for (int i = 0; i < quizData.Quiz.Count; i++)
            {
                quizData.Quiz[i].Id = i;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(quizData, options);
            File.WriteAllText(filePath, updatedJson);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reassigning slide IDs: {ex.Message}");
            return false;
        }
    }



    public static QuizSlide GetSlideById(string quizTitle, int id)
    {
        QuizSlide returnSlide = new QuizSlide();
        List<QuizSlide>? slides = GetQuizData(quizTitle).Quiz;

        if(slides != null)
        {
             foreach(QuizSlide slide in slides)
            {
                if (slide.Id == id) 
                {
                    returnSlide = slide;
                    break;
                }
            }

            return returnSlide;
        }
        else
        {
            Console.WriteLine($"Quizslides of quiz '{quizTitle}' not found.");
            return new QuizSlide();   
        }
    }

    public static QuizData GetQuizData(string quizTitle)
    {
        var filePath = GetFileNameByTitle(quizTitle);

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"Quiz '{quizTitle}' not found.");
            return new QuizData();
        }

        try
        {
            var json = File.ReadAllText(filePath);
            QuizData? quizData = JsonSerializer.Deserialize<QuizData>(json);
            return quizData ?? new QuizData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading quiz '{quizTitle}': {ex.Message}");
            return new QuizData();
        }
    }

    public static string GetFileNameByTitle(string title)
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        if (!Directory.Exists(quizPath))
            return string.Empty;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedTitle = ExtractVarFromFile(file, "title");
            if (extractedTitle == title) return file;
        }

        Console.WriteLine($"Quiz '{title}' not found.");

        return string.Empty;
    }

    public static List<string> GetAllQuizTitles()
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        var titles = new List<string>();

        if (!Directory.Exists(quizPath))
            return titles;

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedTitle = ExtractVarFromFile(file, "title");
            if (!string.IsNullOrEmpty(extractedTitle)) titles.Add(extractedTitle);
        }

        return titles;
    }

    public static async Task PickAndStoreImage()
    {
        var mainWindow =
            (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
            ?.MainWindow;

        if (mainWindow?.StorageProvider == null)
            return;

        var files = await mainWindow.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select an image",
                AllowMultiple = true,
                FileTypeFilter =
                [
                    new FilePickerFileType("Images")
                    {
                        Patterns = ["*.png", "*.jpg", "*.jpeg", "*.webp"]
                    }
                ]
            });

        if(files.Count == 0) return;

        Directory.CreateDirectory(SlideImageStoragePath);

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.Name);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var destinationPath = Path.Combine(SlideImageStoragePath, fileName);

            await using var sourceStream = await file.OpenReadAsync();
            await using var targetStream = File.Create(destinationPath);
            await sourceStream.CopyToAsync(targetStream);
        }
    }

    public static List<string> GetAllSlideImagePaths()
    {
        if (!Directory.Exists(SlideImageStoragePath))
            return new List<string>();

        string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".webp" };

        return Directory
            .EnumerateFiles(SlideImageStoragePath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
            .ToList();
    }

    public static Bitmap GetCachedBitmap(string path)
    {
        if (_bitmapCache.TryGetValue(path, out var bmp))
            return bmp;

        using var stream = File.OpenRead(path);
        bmp = Bitmap.DecodeToWidth(stream, 160);
        _bitmapCache[path] = bmp;
        return bmp;
    }

    public static bool DeleteSlideImage(string path)
    {
        try
        {
            // Remove from cache first
            if (_bitmapCache.TryGetValue(path, out var bitmap))
            {
                bitmap.Dispose();
                _bitmapCache.Remove(path);
            }

            // Delete file from disk
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete image '{path}': {ex.Message}");
            return false;
        }
    }

    private static int GetFirstAvailibleQuizId()
    {
        var quizPath = Path.Combine(Environment.CurrentDirectory, "Data", "Quizzen");

        var ids = new List<int>();

        foreach (var file in Directory.GetFiles(quizPath, "*.json"))
        {
            var extractedId = ExtractVarFromFile(file, "id");
            if (!string.IsNullOrEmpty(extractedId) && int.TryParse(extractedId, out int id))
            {
                ids.Add(id);
            }
        }

        ids.Sort();

        int expected = 1;
        foreach (var id in ids)
        {
            if (id == expected) expected++;
            else break;
        }

        return expected;
    }


    private static string? ExtractVarFromFile(string file, string varName)
    {
        try
        {
            var json = File.ReadAllText(file);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty(varName, out var element))
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.Number:
                        return element.GetInt32().ToString();
                }
            }
            else
            {
                Console.WriteLine($"No '{varName}' found in {file}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {file}: {ex.Message}");
        }

        return null;
    }
}
