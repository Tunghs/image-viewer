namespace ImageViewer.Data
{
    public class FixedSizeStack<T>
    {
        private T[] _buffer;
        private int top = 0;

        public FixedSizeStack(int lenght)
        {
            _buffer = new T[lenght];
        }
        
        public void Push(T item)
        {
            if (top < _buffer.Length)
            {
                _buffer[top] = item;
                top = top + 1;
            }
            else
            {
                for (int index = 0; index < _buffer.Length - 1; index++)
                {
                    _buffer[index] = _buffer[index + 1];
                }
                _buffer[top] = item;
            }
        }

        public T Pop()
        {
            if (top == 0)
            {
                return default;
            }

            T item = _buffer[top];
            top = top - 1;
            return item;
        }
    }
}
