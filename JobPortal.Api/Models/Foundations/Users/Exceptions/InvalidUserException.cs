﻿namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class InvalidUserException : Xeption
    {
        public InvalidUserException()
            : base(message: "User is invalid") { }
    }
}
