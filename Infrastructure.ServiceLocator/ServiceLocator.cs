// <copyright file="ServiceLocator.cs" company="Beckman Coulter">
// Copyright (c) Beckman Coulter. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Authoring.Infrastructure.ServiceLocator
{
    /// <summary>
    /// Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    internal class ServiceLocator : IServiceLocator, IDisposable
    {
        /// <summary>
        /// It is used to create services within a scope.
        /// </summary>
        private readonly IServiceScope serviceScope;

        /// <summary>
        /// Flag to indicate whether Dispose method has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="scopeId">The id of the service scope instance.</param>
        /// <param name="scope">A <see cref="IServiceProvider"/> instance.</param>
        public ServiceLocator(int scopeId, IServiceScope scope)
        {
            ScopeId = scopeId;
            serviceScope = scope;
        }

        /// <summary>
        /// The identity matches with a service scope instance.
        /// </summary>
        public int ScopeId { get; set; }

        /// <summary>
        /// Get service of type T.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type T or null if there is no such service.</returns>
        public T GetService<T>()
        {
            var service = serviceScope != null ? serviceScope.ServiceProvider.GetService<T>() : default(T);
            if (service is IScopeIdProvider sp)
            {
                sp.ScopeId = ScopeId;
            }

            return service;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Indicates whether to free managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                serviceScope?.Dispose();
            }

            disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        ~ServiceLocator()
        {
            Dispose(false);
        }
    }
}
