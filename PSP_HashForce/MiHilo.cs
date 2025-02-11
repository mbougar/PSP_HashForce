namespace PSP_HashForce;

public class MiHilo
{
    private readonly Thread _hilo;
    private readonly string _nombre;
    private readonly string _passwordToFind;
    private readonly Wrapper<bool> _stopThread;
    private readonly List<string> _passwords;
    private ShaHasher _hasher = new ShaHasher();

    public MiHilo(string nombre, string passwordToFind, List<string> passwords, Wrapper<bool> stopFlag)
    {
        _nombre = nombre;
        _passwordToFind = passwordToFind;
        _stopThread = stopFlag;
        _passwords = passwords;
        _hilo = new Thread(_process);
    }

    public void Start()
    {
        _hilo.Start();
    }

    private void _process()
    {
        foreach (var password in _passwords)
        {
            if (_stopThread.Value)
            {
                //Console.WriteLine($"Hilo {_nombre} parado.");
                return;
            }

            if (_passwordToFind == _hasher.GetStringSha256Hash(password))
            {
                _stopThread.Value = true;
                Console.WriteLine($"Contraseña encontrada ({_passwordToFind}) por el hilo {_nombre}.");
            }
        }
    }
}