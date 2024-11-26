using System;
using System.Collections.Generic;

namespace testSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> balls = Solver.getBalls([98, 300, 202], 120);

            foreach (var ball in balls)
            {
                Console.Write($"{ball} ");
            }
        }
    }

    static class Solver
    {
        public static List<int> getBalls(List<int> balls, int capacity)
        {
            double sum = 0;
            var result = new List<int>(balls.Count);
            
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i] < 0) throw new ArgumentOutOfRangeException();

                sum += balls[i];
            }

            //самый простой и быстрый вариант
            //минус в том, что не для всех вариантов мешок окажется полностью заполнен
            // for (int i = 0; i < balls.Count; i++)
            // {
            //     double cont = (balls[i] / sum) * capacity;
            //     
            //     result.Add((int)cont);
            // }

            //вариант в котором всё оставшееся свободное место отдаётся под шары последнего вида
            //проигрыш в производительности не сильный
            //минус в том, что не учитываются пропорции в последнем шаге
            // int totalCount = 0;
            // for (int i = 0; i < balls.Count - 1; i++)
            // {
            //     int cont = (int)((balls[i] / sum) * capacity);
            //     totalCount += cont;
            //     
            //     result.Add(cont);
            // }
            //
            // result.Add(capacity - totalCount);
            
            //самый медленный вариант, но он даёт наиболее точный результат
            //использовал priority queue, чтобы отдать оставшееся свободное место под шары с наибольшей дробной частью в пропорции
            var comparer = Comparer<HeapNode>.Create((HeapNode l, HeapNode r) => l.FractionalPart.CompareTo(r.FractionalPart));
            var heap = new Heap<HeapNode>(comparer);
            int totalCount = 0;
            
            for (int i = 0; i < balls.Count; i++)
            {
                double cont = (balls[i] / sum) * capacity;
                result.Add((int)cont);
                totalCount += result[i];
                
                heap.Add(new HeapNode()
                {
                    Index = i,
                    FractionalPart = cont - result[i]
                });
            }

            for (int i = totalCount; i < capacity; i++)
            {
                HeapNode node = heap.GetTop();
                result[node.Index]++;
            }
            
            return result;
        }

        private struct HeapNode
        {
            public int Index { get; init; }
            
            public double FractionalPart { get; init; }
        }
    }
}