using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection.Metadata.Ecma335;

/// <summary>
/// Custom Migration Annotation Provider
/// write the Attributes to the migration file
/// </summary>
public class CustomMigrationsAnnotationProvider : IMigrationsAnnotationProvider
{
    public CustomMigrationsAnnotationProvider(MigrationsAnnotationProviderDependencies dependencies)
    {
    }

    public IEnumerable<IAnnotation> For(IModel model) => model.GetAnnotations();

    public IEnumerable<IAnnotation> For(IEntityType entityType)
    {
        foreach (var annotation in entityType.GetAnnotations())
        {
            yield return annotation;
        }
    }

    public IEnumerable<IAnnotation> For(IProperty property) => property.GetAnnotations();

    public IEnumerable<IAnnotation> For(IForeignKey foreignKey) => foreignKey.GetAnnotations();

    public IEnumerable<IAnnotation> For(IIndex index) => index.GetAnnotations();

    public IEnumerable<IAnnotation> For(IKey key) => key.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(IRelationalModel model)
    {
        foreach (var annotation in model.GetAnnotations())
        {
            yield return annotation;
        }
    }

    public IEnumerable<IAnnotation> ForRemove(ITable table)
    {
        foreach (var annotation in table.GetAnnotations())
        {
            yield return annotation;
        }
    }

    public IEnumerable<IAnnotation> ForRemove(IColumn column) => column.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(IView view) => view.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(IViewColumn column) => column.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(IUniqueConstraint constraint) => constraint.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(ITableIndex index) => index.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(IForeignKeyConstraint foreignKey) => foreignKey.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(ISequence sequence) => sequence.GetAnnotations();

    public IEnumerable<IAnnotation> ForRemove(ICheckConstraint checkConstraint) => checkConstraint.GetAnnotations();
}
