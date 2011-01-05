namespace PostmarkDotNet.Specifications
{
    internal abstract class SpecificationBase<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T instance);

        public virtual ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public virtual ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public virtual ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public static ISpecification<T> operator &(SpecificationBase<T> one, ISpecification<T> other)
        {
            return one.And(other);
        }

        public static ISpecification<T> operator |(SpecificationBase<T> one, ISpecification<T> other)
        {
            return one.Or(other);
        }
    }
}