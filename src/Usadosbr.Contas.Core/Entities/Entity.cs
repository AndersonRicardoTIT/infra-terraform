using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;
using Usadosbr.Contas.Core.Common;

namespace Usadosbr.Contas.Core.Entities
{
    public abstract class Entity<T> : IAuditable, IEquatable<Entity<T>> where T : notnull
    {
        public T Id { get; protected init; } = default!;

        [NotMapped]
        public bool Valid => _notifications.Any();

        [NotMapped]
        public bool Invalid => !Valid;

        [NotMapped]
        public IReadOnlyCollection<Notification> Notifications => _notifications;

        [NotMapped]
        private readonly List<Notification> _notifications = new();

        public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            var validationResult = validator.Validate(model);

            foreach (var error in validationResult.Errors)
            {
                _notifications.Add(new Notification(error.ErrorCode, error.ErrorMessage));
            }

            return validationResult.IsValid;
        }

        public bool Equals(Entity<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Entity<T>? left, Entity<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T>? left, Entity<T>? right)
        {
            return !Equals(left, right);
        }
    }
}