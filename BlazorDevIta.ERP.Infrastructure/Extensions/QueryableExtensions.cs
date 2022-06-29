using System.Linq.Expressions;
using System.Reflection;

namespace BlazorDevIta.ERP.Infrastructure.Extensions;

public static class QueryableExtensions
{
    //Proprietà che serve per recuperare le informazioni da un tipo.
    //Si prende il riferimento alle proprietà di un metodo.
    private static readonly MethodInfo OrderByMethod = typeof(Queryable)
        //Vengono recuperati tutti i metodi.
        .GetMethods()
        //Viene preso quello che si chiama in un certo modo.
        .Where(method => method.Name == "OrderBy")
        //Viene preso il metodo che ha due parametri.
        .Where(method => method.GetParameters().Length == 2)
        .Single();

    private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable)
        //Vengono recuperati tutti i metodi.
        .GetMethods()
        //Viene preso quello che si chiama in un certo modo.
        .Where(method => method.Name == "OrderByDescending")
        //Viene preso il metodo che ha due parametri.
        .Where(method => method.GetParameters().Length == 2)
        .Single();

    public static IQueryable<TSource> OrderByProperty<TSource>(this IQueryable<TSource> source, string propertyName)
    {
        //x => x.Date per esempio.

        //Questo costruisce la x a partire da una expression.
        ParameterExpression parameter = Expression.Parameter(typeof(TSource), "x");

        //Viene creata una proprietà => x.Date
        Expression orderByProperty = Expression.Property(parameter, propertyName);

        //Viene costruita la lambda => x => x.Date
        LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });

        //Si invoca il metodo generico.
        MethodInfo genericMethod = OrderByMethod.MakeGenericMethod
            (new[] { typeof(TSource), orderByProperty.Type });

        //Si invoca il vero e proprio metodo.
        object? ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return ret != null ? (IQueryable<TSource>)ret : throw new Exception("invoke failed");
    }

    public static IQueryable<TSource> OrderByPropertyDescending<TSource>(this IQueryable<TSource> source, string propertyName)
    {
        //x => x.Date per esempio.

        //Questo costruisce la x a partire da una expression.
        ParameterExpression parameter = Expression.Parameter(typeof(TSource), "x");

        //Viene creata una proprietà => x.Date
        Expression orderByProperty = Expression.Property(parameter, propertyName);

        //Viene costruita la lambda => x => x.Date
        LambdaExpression lambda = Expression.Lambda(orderByProperty, new[] { parameter });

        //Si invoca il metodo generico.
        MethodInfo genericMethod = OrderByDescendingMethod.MakeGenericMethod
            (new[] { typeof(TSource), orderByProperty.Type });

        //Si invoca il vero e proprio metodo.
        object? ret = genericMethod.Invoke(null, new object[] { source, lambda });
        return ret != null ? (IQueryable<TSource>)ret : throw new Exception("invoke failed");
    }
}
