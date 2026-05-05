namespace ConsoleApp1;

public class TriviaQuestion
{
    public string Question { get; set; }
    public string CorrectAnswer { get; set; }
    public List<string> AllAnswers { get; set; }
    public bool WasMissed { get; set; }

    public TriviaQuestion(string question, string correctAnswer, List<string> allAnswers)
    {
        Question = question;
        CorrectAnswer = correctAnswer;
        AllAnswers = allAnswers;
        WasMissed = false;
    }
}

public class TriviaManager
{
    private List<TriviaQuestion> questions;
    private List<TriviaQuestion> missedQuestions;
    private Random rnd;

    public TriviaManager()
    {
        questions = new List<TriviaQuestion>();
        missedQuestions = new List<TriviaQuestion>();
        rnd = new Random();
    }

    // Reads questions from a text file
    // Format per question (5 lines):
    //   Question text
    //   Correct answer
    //   Wrong answer 1
    //   Wrong answer 2
    //   Wrong answer 3
    //   (blank line between questions)
    public void LoadQuestions(string filePath)
    {
        if (File.Exists(filePath) == false)
        {
            Console.WriteLine("Warning: questions file not found. Using sample questions.");
            LoadSampleQuestions();
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        int i = 0;

        while (i < lines.Length)
        {
            // Skip blank lines
            if (lines[i].Trim() == "")
            {
                i++;
                continue;
            }

            // We need at least 5 lines for a question
            if (i + 4 >= lines.Length)
            {
                break;
            }

            string questionText = lines[i];
            string correctAnswer = lines[i + 1];
            string wrongAnswer1 = lines[i + 2];
            string wrongAnswer2 = lines[i + 3];
            string wrongAnswer3 = lines[i + 4];

            List<string> allAnswers = new List<string>();
            allAnswers.Add(correctAnswer);
            allAnswers.Add(wrongAnswer1);
            allAnswers.Add(wrongAnswer2);
            allAnswers.Add(wrongAnswer3);

            TriviaQuestion newQuestion = new TriviaQuestion(questionText, correctAnswer, allAnswers);
            questions.Add(newQuestion);

            i = i + 6; // move past the 5 lines + 1 blank line
        }

        Console.WriteLine("Loaded " + questions.Count + " trivia questions.");
    }

    // Ask a random question, return true if answered correctly
    public bool AskQuestion()
    {
        if (questions.Count == 0)
        {
            Console.WriteLine("No questions available!");
            return true; // let them through if no questions
        }

        // Pick a random question
        int index = rnd.Next(0, questions.Count);
        TriviaQuestion question = questions[index];

        Console.WriteLine("\n--- TRIVIA ---");
        Console.WriteLine(question.Question);

        // Shuffle the answer choices so correct answer isn't always first
        List<string> shuffled = new List<string>(question.AllAnswers);
        ShuffleList(shuffled);

        for (int i = 0; i < shuffled.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + shuffled[i]);
        }

        Console.Write("Your answer (1-4): ");
        string input = Console.ReadLine();
        int chosen = 0;
        bool validInput = int.TryParse(input, out chosen);

        if (validInput == false || chosen < 1 || chosen > 4)
        {
            Console.WriteLine("Invalid input. Counted as wrong.");
            question.WasMissed = true;
            missedQuestions.Add(question);
            return false;
        }

        string chosenAnswer = shuffled[chosen - 1];

        if (chosenAnswer == question.CorrectAnswer)
        {
            Console.WriteLine("Correct!");
            return true;
        }
        else
        {
            Console.WriteLine("Wrong! The correct answer was: " + question.CorrectAnswer);
            question.WasMissed = true;

            // Only add to missed list once
            if (missedQuestions.Contains(question) == false)
            {
                missedQuestions.Add(question);
            }

            return false;
        }
    }

    // Shows all missed questions and their correct answers at the end
    public void ShowMissedQuestions()
    {
        if (missedQuestions.Count == 0)
        {
            Console.WriteLine("\nYou answered every question correctly!");
            return;
        }

        Console.WriteLine("\n--- MISSED QUESTIONS REPORT ---");

        for (int i = 0; i < missedQuestions.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + missedQuestions[i].Question);
            Console.WriteLine("   Correct answer: " + missedQuestions[i].CorrectAnswer);
        }
    }

    // Simple list shuffle using random swaps
    private void ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void LoadSampleQuestions()
    {
        List<string> answers1 = new List<string>();
        answers1.Add("Paris");
        answers1.Add("London");
        answers1.Add("Berlin");
        answers1.Add("Rome");
        questions.Add(new TriviaQuestion("What is the capital of France?", "Paris", answers1));

        List<string> answers2 = new List<string>();
        answers2.Add("8");
        answers2.Add("6");
        answers2.Add("9");
        answers2.Add("7");
        questions.Add(new TriviaQuestion("How many planets are in our solar system?", "8", answers2));

        List<string> answers3 = new List<string>();
        answers3.Add("Leonardo da Vinci");
        answers3.Add("Michelangelo");
        answers3.Add("Raphael");
        answers3.Add("Picasso");
        questions.Add(new TriviaQuestion("Who painted the Mona Lisa?", "Leonardo da Vinci", answers3));
    }
}