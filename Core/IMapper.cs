namespace StreetOfYourCity.Core;

internal interface IMapper<in TSource, out TTarget>
{
    public TTarget Map(TSource source);
}
