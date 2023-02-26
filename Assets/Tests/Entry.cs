namespace Tests
{
    public readonly struct Entry<T>
    {
        public readonly T Value;
        public readonly double Weight;

        public Entry(T value, double weight)
        {
            Value = value;
            Weight = weight;
        }
    }
}