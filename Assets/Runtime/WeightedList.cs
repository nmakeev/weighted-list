using System;
using System.Collections.Generic;

public class WeightedList<T> where T : IComparable<T>
{
    private Random _random;
    private readonly List<Entry<T>> _list;
    private bool _needUpdateWeights;

    public WeightedList(int seed = 0)
    {
        _list = new List<Entry<T>>();
        _random = new Random(seed);
    }

    public WeightedList(Random random)
    {
        _list = new List<Entry<T>>();
        _random = random;
    }

    public void Add(T value, double weight)
    {
        _list.Add(new Entry<T>
        {
            value = value,
            initialWeight = weight
        });
        _needUpdateWeights = true;
    }

    public void UpdateWeight(T value, double weight)
    {
        for (var i = 0; i < _list.Count; i++)
        {
            if (_list[i].value.CompareTo(value) != 0)
            {
                continue;
            }
            
            var entry = _list[i];
            entry.initialWeight = weight;
            _list[i] = entry;
            _needUpdateWeights = true;
        }
    }

    public T GetRandomElement()
    {
        RecalculateWeights();
        var randomValue = _random.NextDouble();
        var index = FindIndex(randomValue);
        return _list[index - 1].value;
    }

    private int FindIndex(double value)
    {
        var left = 0;
        var right = _list.Count - 1;
        while (left <= right)
        {
            var mid = left + (right - left) / 2;

            if (_list[mid].currentWeight < value)
            {
                left = mid + 1;
            }
            else if (_list[mid].currentWeight > value)
            {
                right = mid - 1;
            }
            else
            {
                return mid;
            }
        }

        return left;
    }

    public void RecalculateWeights()
    {
        if (!_needUpdateWeights)
        {
            return;
        }
        
        _list.Sort((lhs, rhs) => lhs.initialWeight.CompareTo(rhs.initialWeight));
        
        var total = CalculateTotalWeight();
        var current = 0.0;
        for (var i = 0; i < _list.Count; i++)
        {
            var entry = _list[i];
            entry.currentWeight = current;
            _list[i] = entry;

            current += entry.initialWeight / total;
        }

        _needUpdateWeights = false;
    }

    private double CalculateTotalWeight()
    {
        var result = 0.0;
        for (var i = 0; i < _list.Count; i++)
        {
            result += _list[i].initialWeight;
        }
        return result;
    }
}