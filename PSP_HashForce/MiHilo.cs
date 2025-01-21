namespace PSP_HashForce;

public class MiHilo
{
    private Thread hilo;
    private string text;
    private Wrapper<bool> stopThread;

    public MiHilo(string text, Wrapper<bool> stopFlag)
    {
        this.text = text;
        this.stopThread = stopFlag;
        hilo = new Thread(_process);
    }

    public void Start()
    {
        hilo.Start();
    }

    void _process()
    {
        for (int i = 0; i < 1000; i++)
        {
            if (stopThread.Value)
            {
                Console.WriteLine($"Hilo {text} parado.");
                return;
            }

            Console.Write(text);
        }
        stopThread.Value = true;
        Console.WriteLine($"Ha terminado: {text}");
    }
}