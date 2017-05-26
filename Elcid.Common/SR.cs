﻿using Elcid.Common.Localization;

namespace Elcid.Common
{
    internal class SRKind
    {
        internal const string NotInWebCache = "NotInWebCache";
        internal const string NullReference = "NullReference";
        internal const string ArgumentNull = "ArgumentNull";
        internal const string ArgumentInvalid = "ArgumentInvalid";
        internal const string ArgumentOutOfRange = "ArgumentOutOfRange";
        internal const string MethodNotFound = "MethodNotFound";
        internal const string Hour = "Hour";
        internal const string Minute = "Minute";
        internal const string Second = "Second";
        internal const string SerializationError = "SerializationError";
        internal const string DeserializationError = "DeerializationError";
        internal const string SerializationTokenInvalid = "SerializationTokenInvalid";
        internal const string UnableProcessExpression = "UnableProcessExpression";
        internal const string NotInterfaceType = "NotInterfaceType";
        internal const string NotExpectedType = "NotExpectedType";
        internal const string NotConcreteType = "NotConcreteType";
        internal const string NoDefaultConstructor = "NoDefaultConstructor";
        internal const string NoDefaultOrDefaultValueConstructor = "NoDefaultOrDefaultValueConstructor";
        internal const string ConstructorHasParameterOfValueType = "ConstructorHasParameterOfValueType";
        internal const string CollectionHasEmptyItem = "CollectionHasEmptyItem";
        internal const string RecursiveInvoke = "RecursiveInvoke";
        internal const string PropertyValueLengthNotMatch = "PropertyValueLengthNotMatch";
        internal const string UnableSaveCacheItem = "UnableSaveCacheItem";
        internal const string UnableReadCacheItem = "UnableReadCacheItem";
        internal const string JsonExpected = "JsonExpected";
        internal const string JsonExpectedArray = "JsonExpectedArray";
        internal const string JsonNullableType = "JsonNullableType";
        internal const string SerializeError = "SerializeError";
        internal const string DeserializeError = "DeserializeError";
        internal const string ParameterLessConstructor = "ParameterLessConstructor";
        internal const string SourceCanotReadDestCanotWrite = "SourceCanotReadDestCanotWrite";
        internal const string UnableReadConfiguration = "UnableReadConfiguration";
        internal const string LoopReferenceSerialize = "LoopReferenceSerialize";
        internal const string NotNewExpression = "NotNewExpression";
        internal const string InterfaceNoBaseType = "InterfaceNoBaseType";
        internal const string UnableSaveAssembly = "UnableSaveAssembly";
        internal const string UnableLoadAssembly = "UnableLoadAssembly";
        internal const string AopTypeMustNotSeald = "AopTypeMustNotSeald";
        internal const string AopMemberMustBeVirtual = "AopMemberMustBeVirtual";
        internal const string AopCanotCreateProxy = "AopCanotCreateProxy";
        internal const string AopCanotCreateProxy_Sealed = "AopCanotCreateProxy_Sealed";
        internal const string StreamNotSupportRead = "StreamNotSupportRead";
        internal const string StreamNotSupportWrite = "StreamNotSupportWrite";
        internal const string MethodNotOverride = "MethodNotOverride";
    }

    internal class SR
    {
        private static readonly StringResource Resource;
        static SR()
        {
            if (Resource == null)
            {
                Resource = StringResource.Create("Strings", typeof(SR).Assembly);
            }
        }

        internal static string GetString(string kind, params object[] args)
        {
            return Resource.GetString(kind, args);
        }

    }
}
