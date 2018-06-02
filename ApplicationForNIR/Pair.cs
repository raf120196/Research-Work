namespace ApplicationForNIR
{
    class Pair
    {
        private int x;
        private int y;

        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int First
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Second
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public override string ToString()
        {
            return x.ToString() + ", " + y.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            Pair p = (Pair)obj;
            return p.x == x && p.y == y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1852591177;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + First.GetHashCode();
            hashCode = hashCode * -1521134295 + Second.GetHashCode();
            return hashCode;
        }
    }
}
