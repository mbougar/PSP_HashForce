// See https://aka.ms/new-console-template for more information

using PSP_HashForce;

string filePath = "PSP_HashForce/2151220-passwords.txt";

if (File.Exists(filePath))
{
    List<string> passwords = new List<string>(File.ReadAllLines(filePath));
    Console.WriteLine(passwords.Count);
}
else
{
    Console.WriteLine("El archivo no existe.");
}

Wrapper<bool> stopThreads = new Wrapper<bool>(false);

MiHilo t1 = new MiHilo("x", stopThreads);
MiHilo t2 = new MiHilo("y", stopThreads);
MiHilo t3 = new MiHilo("z", stopThreads);

t1.Start();
t2.Start();
t3.Start();