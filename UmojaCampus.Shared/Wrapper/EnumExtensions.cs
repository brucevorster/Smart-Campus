﻿
using System;
using System.ComponentModel;

namespace UmojaCampus.Shared.Wrapper
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes?.Length > 0
                ? attributes[0].Description
                : value.ToString();
        }
    }
}
