﻿using CSharpCompiler.CodeGen.Metadata.Tables;
using CSharpCompiler.CodeGen.Metadata.Tables.Assembly;
using CSharpCompiler.CodeGen.Metadata.Tables.AssemblyRef;
using CSharpCompiler.CodeGen.Metadata.Tables.CustomAttribute;
using CSharpCompiler.CodeGen.Metadata.Tables.Field;
using CSharpCompiler.CodeGen.Metadata.Tables.MemberRef;
using CSharpCompiler.CodeGen.Metadata.Tables.Method;
using CSharpCompiler.CodeGen.Metadata.Tables.Module;
using CSharpCompiler.CodeGen.Metadata.Tables.Parameter;
using CSharpCompiler.CodeGen.Metadata.Tables.StandAloneSig;
using CSharpCompiler.CodeGen.Metadata.Tables.TypeRef;
using CSharpCompiler.CodeGen.Sections.Text;
using CSharpCompiler.Semantics.Metadata;
using CSharpCompiler.Utility;
using System;
using System.Collections.ObjectModel;

namespace CSharpCompiler.CodeGen.Metadata
{
    public sealed class MetadataBuilder
    {
        private AssemblyDefinition _assemblyDef;
        private ILCodeBuffer _ilCode;
        private MetadataContainer _metadata;

        public AssemblyTable AssemblyTable { get; private set; }
        public AssemblyRefTable AssemblyRefTable { get; private set; }
        public CustomAttributeTable CustomAttributeTable { get; private set; }
        public FieldTable FieldTable { get; private set; }
        public MemberRefTable MemberRefTable { get; private set; }
        public MethodTable MethodTable { get; private set; }
        public ModuleTable ModuleTable { get; private set; }
        public ParameterTable ParameterTable { get; private set; }
        public StandAloneSigTable StandAloneSigTable { get; private set; }
        public TypeDefTable TypeDefTable { get; private set; }
        public TypeRefTable TypeRefTable { get; private set; }
        
        private MetadataBuilder(AssemblyDefinition assemblyDef)
        {
            _assemblyDef = assemblyDef;
            _ilCode = new ILCodeBuffer(this);
            _metadata = new MetadataContainer(_ilCode);
            AssemblyTable = GetOrCreate(MetadataTableType.Assembly, () => new AssemblyTable(this));
            AssemblyRefTable = GetOrCreate(MetadataTableType.AssemblyRef, () => new AssemblyRefTable(this));
            CustomAttributeTable = GetOrCreate(MetadataTableType.CustomAttribute, () => new CustomAttributeTable(this));
            FieldTable = GetOrCreate(MetadataTableType.Field, () => new FieldTable(this));
            MemberRefTable = GetOrCreate(MetadataTableType.MemberRef, () => new MemberRefTable(this));
            MethodTable = GetOrCreate(MetadataTableType.Method, () => new MethodTable(this));
            ModuleTable = GetOrCreate(MetadataTableType.Module, () => new ModuleTable(this));
            ParameterTable = GetOrCreate(MetadataTableType.Param, () => new ParameterTable(this));
            StandAloneSigTable = GetOrCreate(MetadataTableType.StandAloneSig, () => new StandAloneSigTable(this));
            TypeDefTable = GetOrCreate(MetadataTableType.TypeDef, () => new TypeDefTable(this));
            TypeRefTable = GetOrCreate(MetadataTableType.TypeRef, () => new TypeRefTable(this));
        }

        public static MetadataContainer Build(AssemblyDefinition assemblyDef)
        {
            return new MetadataBuilder(assemblyDef).Build();
        }

        public ushort RegisterGuid(Guid guid)
        {
            return (ushort)_metadata.Guids.RegisterGuid(guid);
        }

        public ushort RegisterString(string @string)
        {
            if (string.IsNullOrEmpty(@string))
                return 0;

            return (ushort)_metadata.Strings.RegisterString(@string);
        }

        public ushort RegisterUserString(string @string)
        {
            if (@string == null)
                return 0;

            return (ushort)_metadata.UserStrings.RegisterString(@string);
        }

        public ushort RegisterBlob(byte[] bytes)
        {
            return (ushort)_metadata.Blobs.RegisterBlob(bytes);
        }

        public ushort RegisterBlob(ByteBuffer buffer)
        {
            return (ushort)_metadata.Blobs.RegisterBlob(buffer);
        }

        public uint WriteMethod(MethodDefinition methodDef)
        {
            return _metadata.ILCode.WriteMethod(methodDef);
        }

        public ushort WriteSignature(IMethodInfo methodInfo)
        {
            StandAloneSignature signature = StandAloneSignature.GetMethodSignature(methodInfo);
            return RegisterBlob(signature);
        }

        public ushort WriteSignature(CustomAttribute attribute)
        {
            StandAloneSignature signature = StandAloneSignature.GetAttributeSignature(attribute);
            return RegisterBlob(signature);
        }

        public TTable GetOrCreate<TTable>(MetadataTableType type, Func<TTable> factory) where TTable : IMetadataTable
        {
            return _metadata.Tables.GetOrCreate(type, factory);
        }

        public MetadataToken ResolveToken(IMetadataEntity entity)
        {
            if (entity == null) return MetadataToken.Zero;
            if (entity is AssemblyDefinition) return AssemblyTable.GetToken((AssemblyDefinition)entity);
            if (entity is ModuleDefinition) return ModuleTable.GetToken((ModuleDefinition)entity);
            if (entity is TypeDefinition) return TypeDefTable.GetToken((TypeDefinition)entity);
            if (entity is MethodDefinition) return MethodTable.GetToken((MethodDefinition)entity);
            if (entity is FieldDefinition) return FieldTable.GetToken((FieldDefinition)entity);
            if (entity is ParameterDefinition) return ParameterTable.GetToken((ParameterDefinition)entity);
            if (entity is CustomAttribute) return CustomAttributeTable.GetToken((CustomAttribute)entity);
            if (entity is StandAloneSignature) return StandAloneSigTable.GetToken((StandAloneSignature)entity);
            if (entity is AssemblyReference) return AssemblyRefTable.GetToken((AssemblyReference)entity);
            if (entity is TypeReference) return TypeRefTable.GetToken((TypeReference)entity);
            if (entity is MethodReference) return MemberRefTable.GetToken((MethodReference)entity);
            
            throw new NotSupportedException();
        }

        public ushort GetCodedRid(IMetadataEntity entity, CodedTokenType type)
        {
            return GetCodedRid(ResolveToken(entity), type);
        }

        public ushort GetCodedRid(MetadataToken token, CodedTokenType type)
        {
            return (ushort)CodedTokenBuilder.Build(token, type).Value;
        }

        private MetadataContainer Build()
        {
            BuildModule(_assemblyDef.Module);
            BuildAssembly(_assemblyDef);
            BuildTypes(_assemblyDef.Module.Types);

            _metadata.EntryPointToken = ResolveToken(_assemblyDef.EntryPoint);
            _metadata.Tables.Write();

            return _metadata;
        }

        private void BuildModule(ModuleDefinition moduleDef)
        {
            TypeDefTable.AddModuleTypeDefinition();
            ModuleTable.Add(moduleDef);
        }

        private void BuildAssembly(AssemblyDefinition assemblyDef)
        {
            AssemblyTable.Add(assemblyDef);
            CustomAttributeTable.AddRange(assemblyDef.CustomAttributes);
        }

        private void BuildTypes(Collection<TypeDefinition> types)
        {
            TypeDefTable.AddRange(types);
        }
    }
}
