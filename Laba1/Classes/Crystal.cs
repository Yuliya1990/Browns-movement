using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Laba1.Classes
{
    public class Crystal
    {
        public double P {  get; set; }
        public int[] Cells;
        public Atom[] Atoms;
        Thread[] threads;
        object lockObject;

        public Crystal(int N, int K, double p) 
        {
            P = p;
            Cells = new int[N];
            Cells[0] = K;
            Atoms = new Atom[K];
            for (int i = 0; i < K; i++)
            {
                Atom newAtom = new Atom(0);
                Atoms[i] = newAtom;
            }
            threads = new Thread[Atoms.Length];
            lockObject = new object();
        }

        public void StartSimulation()
        {
            for (int i = 0; i < Atoms.Length; i++)
            {
                threads[i] = new Thread(() => SimulateParticle(Atoms[i], i));
                threads[i].Start();
            }
            // Run the simulation for 1 minute (60 seconds)
            Thread.Sleep(600);
        }
        public void SimulateParticle(Atom atom, int threadIndex)
        {
            Random random = new Random();

            while (true)
            {
                // Генеруємо випадкове число m в інтервалі [0, 1]
                double m = random.NextDouble();

                lock (lockObject)
                {
                    if (m > P)
                    {
                        // Рух атома вправо
                        if (atom.Position < Cells.Length - 1)
                        {
                            Cells[atom.Position + 1]++;
                            Cells[atom.Position]--;
                            atom.Position++;
                        }
                    }
                    else
                    {
                        // Рух атома вліво
                        if (atom.Position > 0)
                        {
                            Cells[atom.Position]--;
                            Cells[atom.Position - 1]++;
                            atom.Position--;
                        }
                    }
                    DisplayCrystalState();
                }

                Thread.Sleep(5000); // Затримка для відображення стану кристала кожні 5 секунд
            }
        }

        public void DisplayCrystalState()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Console.Write(Cells[i] + " ");
            }
            Console.WriteLine();
        }

    }
}
