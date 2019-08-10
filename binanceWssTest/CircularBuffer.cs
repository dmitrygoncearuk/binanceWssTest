using System;

namespace binanceWssTest
{
    public class CircularBuffer<T>
    {
        private T[] _buffer;
        private int _head;
        private int _tail;
        public int count { get; set; }

        public CircularBuffer(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("capacity", "must be positive");
            _buffer = new T[capacity];
            _head = capacity - 1;
        }

        public void Input(T item)
        {
            _head = (_head + 1) % _buffer.Length;
            _buffer[_head] = item;
            if (count == _buffer.Length)
                _tail = (_tail + 1) % _buffer.Length;
            else
                count++;
        }

        public T Output()
        {
            var element = _buffer[_tail];
            _buffer[_tail] = default(T);
            _tail = (_tail + 1) % _buffer.Length;
            count--;
            return element;
        }

        public bool IsEmpty()
        {
            return count == 0 ? true : false;
        }
    }
}
