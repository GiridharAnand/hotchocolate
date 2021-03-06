﻿using System;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Types.Descriptors
{
    public class UnionTypeDescriptor
        : DescriptorBase<UnionTypeDefinition>
        , IUnionTypeDescriptor
    {
        protected internal UnionTypeDescriptor(IDescriptorContext context, Type clrType)
            : base(context)
        {
            if (clrType == null)
            {
                throw new ArgumentNullException(nameof(clrType));
            }

            Definition.RuntimeType = clrType;
            Definition.Name = context.Naming.GetTypeName(clrType, TypeKind.Union);
            Definition.Description = context.Naming.GetTypeDescription(clrType, TypeKind.Union);
        }

        protected internal UnionTypeDescriptor(
            IDescriptorContext context,
            UnionTypeDefinition definition)
            : base(context)
        {
            Definition = definition;
        }

        protected UnionTypeDescriptor(IDescriptorContext context)
            : base(context)
        {
            Definition.RuntimeType = typeof(object);
        }

        internal protected override UnionTypeDefinition Definition { get; protected set; } =
            new UnionTypeDefinition();

        protected override void OnCreateDefinition(UnionTypeDefinition definition)
        {
            if (Definition.RuntimeType is { })
            {
                Context.Inspector.ApplyAttributes(
                    Context,
                    this,
                    Definition.RuntimeType);
            }

            base.OnCreateDefinition(definition);
        }

        public IUnionTypeDescriptor SyntaxNode(
            UnionTypeDefinitionNode unionTypeDefinitionNode)
        {
            Definition.SyntaxNode = unionTypeDefinitionNode;
            return this;
        }

        public IUnionTypeDescriptor Name(NameString value)
        {
            Definition.Name = value.EnsureNotEmpty(nameof(value));
            return this;
        }

        public IUnionTypeDescriptor Description(string value)
        {
            Definition.Description = value;
            return this;
        }

        public IUnionTypeDescriptor Type<TObjectType>()
            where TObjectType : ObjectType
        {
            Definition.Types.Add(TypeReference.Create(
                typeof(TObjectType), TypeContext.Output));
            return this;
        }

        public IUnionTypeDescriptor Type<TObjectType>(TObjectType objectType)
            where TObjectType : ObjectType
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }
            Definition.Types.Add(new SchemaTypeReference(
                (ITypeSystemObject)objectType));
            return this;
        }

        public IUnionTypeDescriptor Type(NamedTypeNode objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }
            Definition.Types.Add(new SyntaxTypeReference(
                objectType, TypeContext.Output));
            return this;
        }

        public IUnionTypeDescriptor ResolveAbstractType(
            ResolveAbstractType resolveAbstractType)
        {
            Definition.ResolveAbstractType = resolveAbstractType
               ?? throw new ArgumentNullException(nameof(resolveAbstractType));
            return this;
        }

        public IUnionTypeDescriptor Directive<T>(T directiveInstance)
            where T : class
        {
            Definition.AddDirective(directiveInstance);
            return this;
        }

        public IUnionTypeDescriptor Directive<T>()
            where T : class, new()
        {
            Definition.AddDirective(new T());
            return this;
        }

        public IUnionTypeDescriptor Directive(
            NameString name,
            params ArgumentNode[] arguments)
        {
            Definition.AddDirective(name, arguments);
            return this;
        }

        public static UnionTypeDescriptor New(
            IDescriptorContext context,
            Type clrType) =>
            new UnionTypeDescriptor(context, clrType);

        public static UnionTypeDescriptor New(
            IDescriptorContext context) =>
            new UnionTypeDescriptor(context);

        public static UnionTypeDescriptor FromSchemaType(
            IDescriptorContext context,
            Type schemaType)
        {
            var descriptor = New(context, schemaType);
            descriptor.Definition.RuntimeType = typeof(object);
            return descriptor;
        }

        public static UnionTypeDescriptor From(
            IDescriptorContext context,
            UnionTypeDefinition definition) =>
            new UnionTypeDescriptor(context, definition);
    }
}
