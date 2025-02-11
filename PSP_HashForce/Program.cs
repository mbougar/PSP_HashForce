// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using PSP_HashForce;

const string filePath = "2151220-passwords.txt";

if (File.Exists(filePath))
{
    while (true)
    {
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1 - Ejecutar un intento de forzar contraseña.");
        Console.WriteLine("2 - Ejecutar un test para comprobar el número óptimo de hilos (tarda mucho).");
        Console.WriteLine("0 - Salir.");

        try
        {
            var opcion = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            switch (opcion)
            {
                case 1:
                    Console.WriteLine("Introduzca el numero de hilos que se van a usar (lo óptimo en mi dispositivo es 1 hilo): ");
                    var numHilos = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                    EjecutarForce(numHilos);
                    break;
                case 2:
                    EjecutarTestHilos();
                    break;
                case 0:
                    Console.WriteLine("Saliendo del programa...");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error: El valor ingresado no es válido.");
        }
    }
}
else
{
    Console.WriteLine("El archivo no existe.");
}

void EjecutarTestHilos()
{
    var numHilos = 10;
    
    var diccionario = new Dictionary<int, List<long>>();

    for (int i = 1; i <= 10; i++)
    {
        diccionario[i] = new List<long>();
    }
    
    var passwords = new List<string>(File.ReadAllLines(filePath));

    for (var interaciones = 0; interaciones < 1000; interaciones++)
    {
        for (var j = 1; j <= numHilos; j++)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
        
            var shaHasher = new ShaHasher();
            var random = new Random();
            var numPasswords = passwords.Count;
            var chosenPassword = passwords[random.Next(numPasswords)];
            var hashedPassword = shaHasher.GetStringSha256Hash(chosenPassword);
    
            var sublistSize = numPasswords / j;
            var stopThreads = new Wrapper<bool>(false);
            var lastIndex = 0;
    
            for (var i = 0; i < j; i++)
            {
                var adjustedSublistSize = (i == j - 1) ? numPasswords - lastIndex : sublistSize;
                var sublist = passwords.GetRange(lastIndex, adjustedSublistSize);
        
                var hilo = new MiHilo($"{i}", hashedPassword, sublist, stopThreads);
                hilo.Start();
        
                lastIndex += adjustedSublistSize;
            }
        
            stopwatch.Stop();
            diccionario[j].Add(stopwatch.ElapsedMilliseconds);
        }
        
        Console.WriteLine($"Iteración: {interaciones}");
    }
    
    foreach (var hilo in diccionario)
    {
        var mediaTiempo = 0L;
        foreach (var tiempo in hilo.Value)
        {
            mediaTiempo += tiempo;
        }
        
        mediaTiempo /= hilo.Value.Count;
        
        Console.WriteLine($"Hilo: {hilo.Key}, tiempo medio: {mediaTiempo}");
    }
}

void EjecutarForce(int numHilos)
{
    if (numHilos < 1)
    {
        Console.WriteLine("Error: El número de hilos ingresado es menor a 1.");
        return;
    }
    
    var passwords = new List<string>(File.ReadAllLines(filePath));
    
    var shaHasher = new ShaHasher();
    var random = new Random();
    var numPasswords = passwords.Count;
    var chosenPassword = passwords[random.Next(numPasswords)];
    Console.WriteLine($"La contraseña que hay que buscar es: {chosenPassword}");
    var hashedPassword = shaHasher.GetStringSha256Hash(chosenPassword);
    Console.WriteLine($"La contraseña hasheada es: {hashedPassword}");
    
    var sublistSize = numPasswords / numHilos;
    var stopThreads = new Wrapper<bool>(false);
    var lastIndex = 0;
    
    for (var i = 0; i < numHilos; i++)
    {
        var adjustedSublistSize = (i == numHilos - 1) ? numPasswords - lastIndex : sublistSize;
        var sublist = passwords.GetRange(lastIndex, adjustedSublistSize);
        
        var hilo = new MiHilo($"{i}", hashedPassword, sublist, stopThreads);
        hilo.Start();
        
        lastIndex += adjustedSublistSize;
    }
}
