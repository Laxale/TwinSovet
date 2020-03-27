using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;


namespace TwinSovet.Extensions 
{
    internal static class EnumExtensions 
    {
        public static AttachmentHostType ToAttachmentHostType(this SubjectType subjectType) 
        {
            if(subjectType == SubjectType.None) throw new InvalidOperationException($"Нельзя преобразовать значение '{ subjectType }'");

            switch (subjectType)
            {
                case SubjectType.House:
                    return AttachmentHostType.House;
                case SubjectType.Section:
                    return AttachmentHostType.Section;
                case SubjectType.Floor:
                    return AttachmentHostType.Floor;
                case SubjectType.Flat:
                    return AttachmentHostType.Flat;
                case SubjectType.Aborigen:
                    return AttachmentHostType.Aborigen;
                default:
                    throw new InvalidOperationException($"Значение '{ subjectType }' не ожидается");
            }
        }
    }
}