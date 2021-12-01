using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ex1Reflection
{
    public class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Write("Insira o nome de uma classe: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var type = Type.GetType(input);

                var methods = type?.GetMethods();

                if (methods == null) continue;

                foreach (var method in methods)
                {
                    if (!method.IsPublic 
                        || method.IsStatic 
                        || method.GetParameters().Length > 0 
                        && method.GetParameters().All(x => x.ParameterType.GetTypeInfo() != typeof(string))) 
                        continue;

                    Console.WriteLine($"method: {method.Name}");
                }

                Console.Write("Insira o nome do método para executar: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var userInputedMethod = type.GetMethod(input);

                if (userInputedMethod is null) continue;

                var t = Activator.CreateInstance(type, null);

                if (userInputedMethod.GetParameters().Any())
                {
                    object[] parameters = new object[userInputedMethod.GetParameters().Length];
                    var parameterInfos = userInputedMethod.GetParameters();

                    for (var i = 0; i < parameterInfos.Length; i++)
                    {
                        Console.Write($"Insira o valor para {parameterInfos[i].Name}: ");
                        var value = Console.ReadLine();
                        parameters[i] = value;
                    }

                    userInputedMethod.Invoke(t, parameters);
                }
                else
                {
                    userInputedMethod.Invoke(t, null);
                }
            }
        }
    }

    public class Teste
    {
        public void SayHelloTo(string name) => Console.WriteLine($"Hello {name}!");

        public void CountToTen()
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine(i);
            }
        }
        public static void SayHello() => Console.WriteLine($"Hello guest!");
    }
}
