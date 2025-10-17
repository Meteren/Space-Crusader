using System.Collections.Generic;

public interface IDataProvider<T>
{
    T ProvidedData { get; set; }
    void Provide(T providedData);
}
