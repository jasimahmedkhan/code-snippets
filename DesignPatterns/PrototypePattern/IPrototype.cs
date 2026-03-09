namespace design_patterns.PrototypePattern;

public interface IPrototype<T>
{
    T Clone();
}