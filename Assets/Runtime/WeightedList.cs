using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WeightedList<T> where T : IComparable<T>
{
    private readonly Random _random;
    private readonly List<Entry<T>> _list;
    private double _totalWeight;
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
        _totalWeight += weight;
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
            _totalWeight -= entry.initialWeight;
            
            entry.initialWeight = weight;
            _list[i] = entry;
            _needUpdateWeights = true;
            _totalWeight += weight;
        }
    }

    public T GetRandomElement()
    {
        RecalculateWeights();
        var index = FindIndex(GetRandomWeight());
        return _list[index].value;
    }

    public T GetRandomElementAndRemove()
    {
        RecalculateWeights();
        var index = FindIndex(GetRandomWeight());
        var element = _list[index].value;
        _list.RemoveAt(index);
        _needUpdateWeights = true;
        return element;
    }

    private double GetRandomWeight()
    {
        return _random.NextDouble() * _totalWeight;
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

        return left - 1;
    }

    public void RecalculateWeights()
    {
        if (!_needUpdateWeights)
        {
            return;
        }

        _list.Sort((lhs, rhs) => lhs.initialWeight.CompareTo(rhs.initialWeight));

        var current = 0.0;
        for (var i = 0; i < _list.Count; i++)
        {
            var entry = _list[i];
            entry.currentWeight = current;
            _list[i] = entry;

            current += entry.initialWeight;
        }

        _needUpdateWeights = false;
    }
}