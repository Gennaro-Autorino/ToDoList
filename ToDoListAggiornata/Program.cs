using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// Classe che rappresenta una singola attività (task) della lista.

class TodoTask
{
    public string nome { get; set; }
    public bool statoTask { get; set; }

    public override string ToString()
    {
        return $"{nome} = {(statoTask ? "Completata" : "In sospeso")}";
    }
}

class Program
{
    // Lista di task in memoria
    static List<TodoTask> tasks = new List<TodoTask>();

    // Percorso del file dove salvare le task
    static string path = @"filetxt\lista.txt";

    static void Main()
    {
        // Carica le task dal file all'avvio (se esiste)
        CaricaDaFile();

        bool esci = false;
        while (!esci)
        {
            Console.Clear();
            Console.WriteLine("===== TO-DO LIST =====");
            Console.WriteLine("1) Visualizza le task");
            Console.WriteLine("2) Aggiungi una task");
            Console.WriteLine("3) Rimuovi una task");
            Console.WriteLine("4) Modifica una task");
            Console.WriteLine("5) Cancella il file ed esci");
            Console.WriteLine("6) Esci");
            Console.Write("Scelta: ");

            switch (Console.ReadLine())
            {
                case "1":
                    VisualizzaTask();
                    break;
                case "2":
                    AggiungiTask();
                    break;
                case "3":
                    RimuoviTask();
                    break;
                case "4":
                    ModificaTask();
                    break;
                case "5":
                    CancellaFile(); esci = true;
                    break;
                    
                case "6":
                    // Salva le task su file prima di uscire
                    SalvaSuFile();
                    esci = true;
                    break;
                default:
                    Console.WriteLine("Scelta non valida!");
                    Console.ReadKey();
                    break;
            }
        }

      
    }

    // Visualizza tutte le task nella lista.

    static void VisualizzaTask()
    {
        Console.Clear();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Lista Task Vuota!");
        }
        else
        {
            Console.WriteLine("Elenco delle Task:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }

        Console.ReadKey();
    }

    //Aggiunge una nuova task alla lista.

    static void AggiungiTask()
    {
        Console.Clear();
        Console.Write("nome Task: ");
        string nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("nome non valido!");
        }
        else
        {
            tasks.Add(new TodoTask { nome = nome.Trim(), statoTask = false });
            SalvaSuFile();
            Console.WriteLine("Task aggiunta!");
        }

        Console.ReadKey();
    }


    // Rimuove una task dalla lista.

    static void RimuoviTask()
    {
        Console.Clear();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Lista Task Vuota!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Inserisci il nome della Task da rimuovere:");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }

        string nome = Console.ReadLine();
        var daEliminare = tasks.FirstOrDefault(t =>
            t.nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (daEliminare == null)
        {
            Console.WriteLine("Task non trovata!");
        }
        else
        {
            tasks.Remove(daEliminare);
            SalvaSuFile();
            Console.WriteLine("Task eliminata.");
        }

        Console.ReadKey();
    }

    // Modifica nome e/o stato di una task.

    static void ModificaTask()
    {
        Console.Clear();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Lista Task Vuota!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Inserisci il nome della Task da modificare:");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }

        string nome = Console.ReadLine();
        var taskDaModificare = tasks.FirstOrDefault(t => t.nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (taskDaModificare == null)
        {
            Console.WriteLine("Task non trovata!");
        }
        else
        {
            Console.Write("Vuoi modificare il nome? (si/no): ");
            if (Console.ReadLine().ToLower() == "si")
            {
                Console.Write("Nuovo nome: ");
                taskDaModificare.nome = Console.ReadLine();
            }

            Console.Write("La task è completata? (si/no): ");
            string risposta = Console.ReadLine().ToLower();
            taskDaModificare.statoTask = risposta == "si";

            SalvaSuFile();
            Console.WriteLine("Task modificata!");
        }

        Console.ReadKey();
    }

    // Salva tutte le task nel file di testo con il seguente formato: nometask;statotask
    static void SalvaSuFile()
    {
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.nome};{task.statoTask}");
            }
        }
    }

    // Carica tutte le task dal file di testo.
    static void CaricaDaFile()
    {
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split(';');    // Divide il contenuto di ogni riga del file di testo in 2 parti divise dal ;
            if (parts.Length == 2)          // Se le parti trovate sono 2 procede ad aggiugnere le task alla lista utilizzando come nome parts[0] e come stato task parts[1]
            {
                tasks.Add(new TodoTask
                {
                    nome = parts[0],
                    statoTask = bool.Parse(parts[1])
                });
            }
        }
    }

    //Permette di cancellare il file di testo su cui vengono salvate le task
    static void CancellaFile()
    {   
        //Viene chiesta conferma all'utente
        Console.WriteLine("Cancellare il file? ");
        Console.WriteLine("1) Si");
        Console.WriteLine("2) No");
        switch (Console.ReadLine())
        {
            case "1": File.Delete(path); 
                Console.WriteLine("File cancellato correttamente!");
                Console.ReadKey();
                break;
            case "2": Console.WriteLine("File non cancellato!");
                Console.ReadKey();
                break;
            default: Console.WriteLine("Risposta non valida");
                Console.ReadKey();
                break;
        }
    }
}
