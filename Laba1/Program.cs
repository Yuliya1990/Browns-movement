using System;
using System.Threading;

class CrystalSimulation
{
    static int N = 10; // Кількість клітин в кристалі
    static int K = 10;  // Кількість атомів домішок
    static double p = 0.7; // Імовірність руху вліво або вправо

    static int[] cells = new int[N]; // Масив клітин кристала

    static object lockObject = new object(); // Lock object for synchronization
    static bool isWork = true;

    static void Main(string[] args)
    {
        InitializeCrystal();
        Console.WriteLine("Initial state:");

        
        // Create and start threads for impurity atoms
        Thread[] threads = new Thread[K+1];

        threads[K] = new Thread(() => DisplayCrystalState());
        threads[K].Start();


        for (int i = 0; i < K; i++)
        {
            
            threads[i] = new Thread(() => SimulateParticle(0, i));
            threads[i].Start();
        }

        // Run the simulation for 1 minute (60 seconds)
        Thread.Sleep(20000);
        isWork = false;
    
        // Recalculate and display the total number of impurity atoms
        int recalculatedTotal = 0;
        foreach (int count in cells)
        {
            recalculatedTotal += count;
        }

        Console.WriteLine($"Total impurity atoms: {recalculatedTotal}");
    
    }

    // Логіка руху атома домішки
    static void SimulateParticle(int index, int threadIndex)
    {
        Random random = new Random();

        while (isWork)
        {
            // Генеруємо випадкове число m в інтервалі [0, 1]
            double m = random.NextDouble();
           lock (lockObject)
           {

                if (m > p)
                {
                    // Рух атома вправо
                    if (index < N - 1)
                    {

                        cells[index + 1]++;
                        cells[index]--;
                        index++;
                    }
                }
                else
                {
                    // Рух атома вліво
                    if (index > 0)
                    {
                        cells[index]--;
                        cells[index - 1]++;
                        index--;
                    }
                }
            }

        }
    }

    static void InitializeCrystal()
    {
        // Початково атоми домішок усі зосереджені в крайній лівій клітинці
        cells[0] = K;
    }

    static void DisplayCrystalState()
    {
        while (isWork)
        {
            lock (lockObject)
            {
                for (int i = 0; i < N; i++)
                {
                    Console.Write(cells[i] + " ");
                }
                Console.WriteLine();
            }
            Thread.Sleep(3000);
        }
    }
}
