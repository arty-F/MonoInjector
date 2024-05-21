﻿using Assets.Core.Providers;
using Assets.Core.Resolvers;
using System.Reflection;

namespace Assets.Core
{
    internal class ResolvingStrategy
    {
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private readonly GenericMethodsProvider _genericMethodsProvider;
        private readonly SettersProvider _settersProvider;
        private readonly MemberInfoProvider _memberInfoProvider;
        private readonly InstancesProvider _instancesProvider;
        private readonly ParameterTypesProvider _parameterTypesProvider;
        private readonly FieldResolver _fieldResolver;
        private readonly PropertyResolver _propertyResolver;
        private readonly MethodResolver _methodResolver;

        public ResolvingStrategy(InstancesProvider instancesStorage)
        {
            _genericMethodsProvider = new GenericMethodsProvider();
            _settersProvider = new SettersProvider();
            _memberInfoProvider = new MemberInfoProvider(FLAGS);
            _instancesProvider = instancesStorage;
            _parameterTypesProvider = new ParameterTypesProvider();
            _fieldResolver = new FieldResolver(_genericMethodsProvider, _memberInfoProvider, _settersProvider, _instancesProvider, FLAGS);
            _propertyResolver = new PropertyResolver(_genericMethodsProvider, _memberInfoProvider, _settersProvider, _instancesProvider, FLAGS);
            _methodResolver = new MethodResolver(_genericMethodsProvider, _memberInfoProvider, _instancesProvider, _parameterTypesProvider, FLAGS);
        }

        internal void Resolve(object consumer)
        {
            var consumerType = consumer.GetType();
            _fieldResolver.Resolve(consumer, consumerType);
            _propertyResolver.Resolve(consumer, consumerType);
            _methodResolver.Resolve(consumer, consumerType);
        }
    }
}
