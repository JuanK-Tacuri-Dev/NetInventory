using System.Data;
using Dapper;

namespace NetInventory.Infrastructure.Persistence.ReadModel;

public sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
        => parameter.Value = value.ToString();

    public override Guid Parse(object value)
        => Guid.Parse((string)value);
}

/// <summary>
/// Permite que Dapper convierta REAL/FLOAT/DOUBLE del motor de base de datos
/// a decimal de .NET. Necesario porque SQLite almacena decimales como REAL
/// y Dapper no realiza esta conversión automáticamente.
/// </summary>
public sealed class DecimalTypeHandler : SqlMapper.TypeHandler<decimal>
{
    public override void SetValue(IDbDataParameter parameter, decimal value)
        => parameter.Value = value;

    public override decimal Parse(object value)
        => Convert.ToDecimal(value);
}
