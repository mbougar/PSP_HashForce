// See https://aka.ms/new-console-template for more information

using PSP_HashForce;

const string filePath = "2151220-passwords.txt";
const int numHilos = 10;

if (File.Exists(filePath))
{
    var passwords = new List<string>(File.ReadAllLines(filePath));
    var random = new Random();
    var numPasswords = passwords.Count;
    var chosenPassword = passwords[random.Next(numPasswords)];
    Console.WriteLine($"La contraseña que hay que buscar es: {chosenPassword}");
    
    var sublistSize = numPasswords / numHilos;
    var stopThreads = new Wrapper<bool>(false);
    var lastIndex = 0;
    
    for (int i = 0; i < numHilos; i++)
    {
        var adjustedSublistSize = (i == numHilos - 1) ? numPasswords - lastIndex : sublistSize;
        var sublist = passwords.GetRange(lastIndex, adjustedSublistSize);
        
        var hilo = new MiHilo($"{i}", chosenPassword, sublist, stopThreads);
        hilo.Start();
        
        lastIndex += adjustedSublistSize;
    }
}
else
{
    Console.WriteLine("El archivo no existe.");
}