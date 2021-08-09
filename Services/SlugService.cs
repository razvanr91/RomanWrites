using RomanWrites.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanWrites.Services
{
    public class SlugService : ISlugService
    {
        private readonly ApplicationDbContext _context;

        public SlugService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsUnique(string slug)
        {
            return !_context.Posts.Any(post => post.Slug == slug);
        }

        public string UrlFriendly(string title)
        {
            if ( title is null )
                return "";
            const int maxLength = 80;
            var length = title.Length;
            var prevdash = false;

            var sb = new StringBuilder(length);

            char c;

            for(int i = 0; i < length;i++ )
            {
                c = title[i];
                if((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if(c >= 'A' && c <= 'Z')
                {
                    sb.Append(Char.ToLower(c));
                    prevdash = false;
                }
                else if(c == ' ' || c == ',' || c == '.' || c == '/' ||
                       c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if(!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if(c == '#')
                {
                    if ( i > 0 )
                        if ( title[i - 1] == 'C' || title[i - 1] == 'F' )
                            sb.Append("-sharp");
                }
                else if(c == '+')
                {
                    sb.Append("-plus");
                }
                else if((int) c >= 128)
                {
                    int prevLength = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if ( prevLength != sb.Length )
                        prevdash = false;
                }

                if ( sb.Length == maxLength )
                    break;
            }
            if ( prevdash )
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        private string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ( "àåáâäãåą".Contains(s) )
            {
                return "a";
            }
            else if ( "èéêëę".Contains(s) )
            {
                return "e";
            }
            else if ( "ìíîïı".Contains(s) )
            {
                return "i";
            }
            else if ( "òóôõöøőð".Contains(s) )
            {
                return "o";
            }
            else if ( "ùúûüŭů".Contains(s) )
            {
                return "u";
            }
            else if ( "çćčĉ".Contains(s) )
            {
                return "c";
            }
            else if ( "żźž".Contains(s) )
            {
                return "z";
            }
            else if ( "śşšŝ".Contains(s) )
            {
                return "s";
            }
            else if ( "ñń".Contains(s) )
            {
                return "n";
            }
            else if ( "ýÿ".Contains(s) )
            {
                return "y";
            }
            else if ( "ğĝ".Contains(s) )
            {
                return "g";
            }
            else if ( c == 'ř' )
            {
                return "r";
            }
            else if ( c == 'ł' )
            {
                return "l";
            }
            else if ( c == 'đ' )
            {
                return "d";
            }
            else if ( c == 'ß' )
            {
                return "ss";
            }
            else if ( c == 'Þ' )
            {
                return "th";
            }
            else if ( c == 'ĥ' )
            {
                return "h";
            }
            else if ( c == 'ĵ' )
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}
