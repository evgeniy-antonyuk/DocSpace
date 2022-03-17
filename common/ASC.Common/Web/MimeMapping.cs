/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/

namespace ASC.Common.Web;

public static class MimeMapping
{
    private static readonly Hashtable _extensionToMimeMappingTable = new Hashtable(200, StringComparer.CurrentCultureIgnoreCase);
    private static readonly IDictionary<string, IList<string>> _mimeSynonyms = new Dictionary<string, IList<string>>();

    static MimeMapping()
    {
        AddMimeMapping(".323", "text/h323");
        AddMimeMapping(".3dm", "x-world/x-3dmf");
        AddMimeMapping(".3dmf", "x-world/x-3dmf");
        AddMimeMapping(".a", "application/octet-stream");
        AddMimeMapping(".aab", "application/x-authorware-bin");
        AddMimeMapping(".aac", "audio/x-aac");
        AddMimeMapping(".aam", "application/x-authorware-map");
        AddMimeMapping(".aas", "application/x-authorware-seg");
        AddMimeMapping(".abc", "text/vnd.abc");
        AddMimeMapping(".acgi", "text/html");
        AddMimeMapping(".acx", "application/internet-property-stream");
        AddMimeMapping(".afl", "video/animaflex");
        AddMimeMapping(".ai", "application/postscript");
        AddMimeMapping(".aif", "audio/aiff");
        AddMimeMapping(".aif", "audio/x-aiff");
        AddMimeMapping(".aifc", "audio/aiff");
        AddMimeMapping(".aifc", "audio/x-aiff");
        AddMimeMapping(".aiff", "audio/aiff");
        AddMimeMapping(".aiff", "audio/x-aiff");
        AddMimeMapping(".aim", "application/x-aim");
        AddMimeMapping(".aip", "text/x-audiosoft-intra");
        AddMimeMapping(".ani", "application/x-navi-animation");
        AddMimeMapping(".aos", "application/x-nokia-9000-communicator-add-on-software");
        AddMimeMapping(".application", "application/x-ms-application");
        AddMimeMapping(".aps", "application/mime");
        AddMimeMapping(".arc", "application/octet-stream");
        AddMimeMapping(".arj", "application/arj");
        AddMimeMapping(".arj", "application/octet-stream");
        AddMimeMapping(".art", "image/x-jg");
        AddMimeMapping(".asf", "video/x-ms-asf");
        AddMimeMapping(".asm", "text/x-asm");
        AddMimeMapping(".asp", "text/asp");
        AddMimeMapping(".asr", "video/x-ms-asf");
        AddMimeMapping(".asx", "application/x-mplayer2");
        AddMimeMapping(".asx", "video/x-ms-asf");
        AddMimeMapping(".asx", "video/x-ms-asf-plugin");
        AddMimeMapping(".au", "audio/basic");
        AddMimeMapping(".au", "audio/x-au");
        AddMimeMapping(".avi", "video/avi");
        AddMimeMapping(".avi", "application/x-troff-msvideo");
        AddMimeMapping(".avi", "video/msvideo");
        AddMimeMapping(".avi", "video/x-msvideo");
        AddMimeMapping(".avs", "video/avs-video");
        AddMimeMapping(".axs", "application/olescript");
        AddMimeMapping(".bas", "text/plain");
        AddMimeMapping(".bcpio", "application/x-bcpio");
        AddMimeMapping(".bin", "application/octet-stream");
        AddMimeMapping(".bin", "application/mac-binary");
        AddMimeMapping(".bin", "application/macbinary");
        AddMimeMapping(".bin", "application/x-binary");
        AddMimeMapping(".bin", "application/x-macbinary");
        AddMimeMapping(".bm", "image/bmp");
        AddMimeMapping(".bmp", "image/bmp");
        AddMimeMapping(".bmp", "image/x-windows-bmp");
        AddMimeMapping(".bmp", "image/x-ms-bmp");
        AddMimeMapping(".boo", "application/book");
        AddMimeMapping(".book", "application/book");
        AddMimeMapping(".boz", "application/x-bzip2");
        AddMimeMapping(".bsh", "application/x-bsh");
        AddMimeMapping(".bz", "application/x-bzip");
        AddMimeMapping(".bz2", "application/x-bzip2");
        AddMimeMapping(".c", "text/plain");
        AddMimeMapping(".c", "text/x-c");
        AddMimeMapping(".c++", "text/plain");
        AddMimeMapping(".cat", "application/vnd.ms-pki.seccat");
        AddMimeMapping(".cat", "application/vndms-pkiseccat");
        AddMimeMapping(".cc", "text/plain");
        AddMimeMapping(".cc", "text/x-c");
        AddMimeMapping(".ccad", "application/clariscad");
        AddMimeMapping(".cco", "application/x-cocoa");
        AddMimeMapping(".cdf", "application/cdf");
        AddMimeMapping(".cdf", "application/x-cdf");
        AddMimeMapping(".cdf", "application/x-netcdf");
        AddMimeMapping(".cer", "application/pkix-cert");
        AddMimeMapping(".cer", "application/x-x509-ca-cert");
        AddMimeMapping(".cha", "application/x-chat");
        AddMimeMapping(".chat", "application/x-chat");
        AddMimeMapping(".class", "application/java");
        AddMimeMapping(".class", "application/java-byte-code");
        AddMimeMapping(".class", "application/x-java-class");
        AddMimeMapping(".clp", "application/x-msclip");
        AddMimeMapping(".cmx", "image/x-cmx");
        AddMimeMapping(".cod", "image/cis-cod");
        AddMimeMapping(".com", "application/octet-stream");
        AddMimeMapping(".com", "text/plain");
        AddMimeMapping(".conf", "text/plain");
        AddMimeMapping(".cpio", "application/x-cpio");
        AddMimeMapping(".cpp", "text/x-c");
        AddMimeMapping(".cpt", "application/mac-compactpro");
        AddMimeMapping(".cpt", "application/x-compactpro");
        AddMimeMapping(".cpt", "application/x-cpt");
        AddMimeMapping(".crd", "application/x-mscardfile");
        AddMimeMapping(".crl", "application/pkcs-crl");
        AddMimeMapping(".crl", "application/pkix-crl");
        AddMimeMapping(".crt", "application/pkix-cert");
        AddMimeMapping(".crt", "application/x-x509-ca-cert");
        AddMimeMapping(".crt", "application/x-x509-user-cert");
        AddMimeMapping(".csh", "application/x-csh");
        AddMimeMapping(".csh", "text/x-script.csh");
        AddMimeMapping(".css", "text/css");
        AddMimeMapping(".css", "application/x-pointplus");
        AddMimeMapping(".csv", "text/csv");
        AddMimeMapping(".cxx", "text/plain");
        AddMimeMapping(".dcr", "application/x-director");
        AddMimeMapping(".deepv", "application/x-deepv");
        AddMimeMapping(".def", "text/plain");
        AddMimeMapping(".deploy", "application/octet-stream");
        AddMimeMapping(".der", "application/x-x509-ca-cert");
        AddMimeMapping(".dib", "image/bmp");
        AddMimeMapping(".dif", "video/x-dv");
        AddMimeMapping(".dir", "application/x-director");
        AddMimeMapping(".disco", "text/xml");
        AddMimeMapping(".djvu", "image/vnd.djvu");
        AddMimeMapping(".dll", "application/octet-stream");
        AddMimeMapping(".dll", "application/x-msdownload");
        AddMimeMapping(".dl", "video/dl");
        AddMimeMapping(".dl", "video/x-dl");
        AddMimeMapping(".doc", "application/msword");
        AddMimeMapping(".docm", "application/vnd.ms-word.document.macroEnabled.12");
        AddMimeMapping(".doct", "application/doct");
        AddMimeMapping(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        AddMimeMapping(".docxf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        AddMimeMapping(".docxf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.docxf");
        AddMimeMapping(".dot", "application/msword");
        AddMimeMapping(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
        AddMimeMapping(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
        AddMimeMapping(".dp", "application/commonground");
        AddMimeMapping(".drw", "application/drafting");
        AddMimeMapping(".dump", "application/octet-stream");
        AddMimeMapping(".dv", "video/x-dv");
        AddMimeMapping(".dvi", "application/x-dvi");
        AddMimeMapping(".dwf", "drawing/x-dwf (old)");
        AddMimeMapping(".dwf", "model/vnd.dwf");
        AddMimeMapping(".dwg", "application/acad");
        AddMimeMapping(".dwg", "image/vnd.dwg");
        AddMimeMapping(".dwg", "image/x-dwg");
        AddMimeMapping(".dxf", "application/dxf");
        AddMimeMapping(".dxf", "image/vnd.dwg");
        AddMimeMapping(".dxf", "image/x-dwg");
        AddMimeMapping(".dxr", "application/x-director");
        AddMimeMapping(".el", "text/x-script.elisp");
        AddMimeMapping(".elc", "application/x-bytecode.elisp (compiled elisp)");
        AddMimeMapping(".elc", "application/x-elc");
        AddMimeMapping(".env", "application/x-envoy");
        AddMimeMapping(".eml", "message/rfc822");
        AddMimeMapping(".eps", "application/postscript");
        AddMimeMapping(".epub", "application/epub+zip");
        AddMimeMapping(".es", "application/x-esrehber");
        AddMimeMapping(".etx", "text/x-setext");
        AddMimeMapping(".evy", "application/envoy");
        AddMimeMapping(".evy", "application/x-envoy");
        AddMimeMapping(".exe", "application/octet-stream");
        AddMimeMapping(".f", "text/plain");
        AddMimeMapping(".f", "text/x-fortran");
        AddMimeMapping(".f4v", "video/x-f4v");
        AddMimeMapping(".f77", "text/x-fortran");
        AddMimeMapping(".f90", "text/plain");
        AddMimeMapping(".f90", "text/x-fortran");
        AddMimeMapping(".fb2", "text/xml");
        AddMimeMapping(".fdf", "application/vnd.fdf");
        AddMimeMapping(".fif", "application/fractals");
        AddMimeMapping(".fif", "image/fif");
        AddMimeMapping(".flac", "audio/flac");
        AddMimeMapping(".fli", "video/fli");
        AddMimeMapping(".fli", "video/x-fli");
        AddMimeMapping(".flo", "image/florian");
        AddMimeMapping(".flr", "x-world/x-vrml");
        AddMimeMapping(".flx", "text/vnd.fmi.flexstor");
        AddMimeMapping(".fmf", "video/x-atomic3d-feature");
        AddMimeMapping(".fodp", "application/vnd.oasis.opendocument.presentation");
        AddMimeMapping(".fods", "application/vnd.oasis.opendocument.spreadsheet");
        AddMimeMapping(".fodt", "application/vnd.oasis.opendocument.text");
        AddMimeMapping(".for", "text/plain");
        AddMimeMapping(".for", "text/x-fortran");
        AddMimeMapping(".fpx", "image/vnd.fpx");
        AddMimeMapping(".fpx", "image/vnd.net-fpx");
        AddMimeMapping(".frl", "application/freeloader");
        AddMimeMapping(".funk", "audio/make");
        AddMimeMapping(".g", "text/plain");
        AddMimeMapping(".g3", "image/g3fax");
        AddMimeMapping(".gdoc", "application/vnd.google-apps.document");
        AddMimeMapping(".gdraw", "application/vnd.google-apps.drawing");
        AddMimeMapping(".gif", "image/gif");
        AddMimeMapping(".gl", "video/gl");
        AddMimeMapping(".gl", "video/x-gl");
        AddMimeMapping(".gsd", "audio/x-gsm");
        AddMimeMapping(".gsheet", "application/vnd.google-apps.spreadsheet");
        AddMimeMapping(".gslides", "application/vnd.google-apps.presentation");
        AddMimeMapping(".gsm", "audio/x-gsm");
        AddMimeMapping(".gsp", "application/x-gsp");
        AddMimeMapping(".gss", "application/x-gss");
        AddMimeMapping(".gtar", "application/x-gtar");
        AddMimeMapping(".gz", "application/x-gzip");
        AddMimeMapping(".gz", "application/x-compressed");
        AddMimeMapping(".gzip", "application/x-gzip");
        AddMimeMapping(".gzip", "multipart/x-gzip");
        AddMimeMapping(".h", "text/plain");
        AddMimeMapping(".h", "text/x-h");
        AddMimeMapping(".hdf", "application/x-hdf");
        AddMimeMapping(".help", "application/x-helpfile");
        AddMimeMapping(".hgl", "application/vnd.hp-hpgl");
        AddMimeMapping(".hh", "text/plain");
        AddMimeMapping(".hh", "text/x-h");
        AddMimeMapping(".hlb", "text/x-script");
        AddMimeMapping(".hlp", "application/hlp");
        AddMimeMapping(".hlp", "application/x-helpfile");
        AddMimeMapping(".hlp", "application/x-winhelp");
        AddMimeMapping(".hpg", "application/vnd.hp-hpgl");
        AddMimeMapping(".hpgl", "application/vnd.hp-hpgl");
        AddMimeMapping(".hqx", "application/binhex");
        AddMimeMapping(".hqx", "application/binhex4");
        AddMimeMapping(".hqx", "application/mac-binhex");
        AddMimeMapping(".hqx", "application/mac-binhex40");
        AddMimeMapping(".hqx", "application/x-binhex40");
        AddMimeMapping(".hqx", "application/x-mac-binhex40");
        AddMimeMapping(".hta", "application/hta");
        AddMimeMapping(".htc", "text/x-component");
        AddMimeMapping(".htm", "text/html");
        AddMimeMapping(".html", "text/html");
        AddMimeMapping(".htmls", "text/html");
        AddMimeMapping(".htt", "text/webviewhtml");
        AddMimeMapping(".htx", "text/html");
        AddMimeMapping(".ice", "x-conference/x-cooltalk");
        AddMimeMapping(".ico", "image/x-icon");
        AddMimeMapping(".idc", "text/plain");
        AddMimeMapping(".ief", "image/ief");
        AddMimeMapping(".iefs", "image/ief");
        AddMimeMapping(".iii", "application/x-iphone");
        AddMimeMapping(".iges", "application/iges");
        AddMimeMapping(".iges", "model/iges");
        AddMimeMapping(".igs", "application/iges");
        AddMimeMapping(".igs", "model/iges");
        AddMimeMapping(".ima", "application/x-ima");
        AddMimeMapping(".imap", "application/x-httpd-imap");
        AddMimeMapping(".inf", "application/inf");
        AddMimeMapping(".ins", "application/x-internett-signup");
        AddMimeMapping(".ip", "application/x-ip2");
        AddMimeMapping(".isp", "application/x-internet-signup");
        AddMimeMapping(".isu", "video/x-isvideo");
        AddMimeMapping(".it", "audio/it");
        AddMimeMapping(".iv", "application/x-inventor");
        AddMimeMapping(".ivf", "video/x-ivf");
        AddMimeMapping(".ivr", "i-world/i-vrml");
        AddMimeMapping(".ivy", "application/x-livescreen");
        AddMimeMapping(".jam", "audio/x-jam");
        AddMimeMapping(".jav", "text/plain");
        AddMimeMapping(".jav", "text/x-java-source");
        AddMimeMapping(".java", "text/plain");
        AddMimeMapping(".java", "text/x-java-source");
        AddMimeMapping(".jcm", "application/x-java-commerce");
        AddMimeMapping(".jfif", "image/jpeg");
        AddMimeMapping(".jfif", "image/pjpeg");
        AddMimeMapping(".jfif-tbnl", "image/jpeg");
        AddMimeMapping(".jpeg", "image/jpeg");
        AddMimeMapping(".jpe", "image/jpeg");
        AddMimeMapping(".jpe", "image/pjpeg");
        AddMimeMapping(".jpeg", "image/pjpeg");
        AddMimeMapping(".jpg", "image/jpeg");
        AddMimeMapping(".jpg", "image/pjpeg");
        AddMimeMapping(".jps", "image/x-jps");
        AddMimeMapping(".json", "application/json");
        AddMimeMapping(".js", "text/javascript");
        AddMimeMapping(".js", "application/javascript");
        AddMimeMapping(".js", "application/x-javascript");
        AddMimeMapping(".js", "application/ecmascript");
        AddMimeMapping(".js", "text/ecmascript");
        AddMimeMapping(".jut", "image/jutvision");
        AddMimeMapping(".kar", "audio/midi");
        AddMimeMapping(".kar", "music/x-karaoke");
        AddMimeMapping(".ksh", "application/x-ksh");
        AddMimeMapping(".ksh", "text/x-script.ksh");
        AddMimeMapping(".la", "audio/nspaudio");
        AddMimeMapping(".la", "audio/x-nspaudio");
        AddMimeMapping(".lam", "audio/x-liveaudio");
        AddMimeMapping(".latex", "application/x-latex");
        AddMimeMapping(".less", "text/css");
        AddMimeMapping(".lha", "application/lha");
        AddMimeMapping(".lha", "application/octet-stream");
        AddMimeMapping(".lha", "application/x-lha");
        AddMimeMapping(".lhx", "application/octet-stream");
        AddMimeMapping(".list", "text/plain");
        AddMimeMapping(".lma", "audio/nspaudio");
        AddMimeMapping(".lma", "audio/x-nspaudio");
        AddMimeMapping(".log", "text/plain");
        AddMimeMapping(".lsf", "video/x-la-asf");
        AddMimeMapping(".lsp", "application/x-lisp");
        AddMimeMapping(".lsp", "text/x-script.lisp");
        AddMimeMapping(".lst", "text/plain");
        AddMimeMapping(".lsx", "text/x-la-asf");
        AddMimeMapping(".ltx", "application/x-latex");
        AddMimeMapping(".lzh", "application/octet-stream");
        AddMimeMapping(".lzh", "application/x-lzh");
        AddMimeMapping(".lzx", "application/lzx");
        AddMimeMapping(".lzx", "application/octet-stream");
        AddMimeMapping(".lzx", "application/x-lzx");
        AddMimeMapping(".m", "text/plain");
        AddMimeMapping(".m", "text/x-m");
        AddMimeMapping(".m13", "application/x-msmediaview");
        AddMimeMapping(".m14", "application/x-msmediaview");
        AddMimeMapping(".m1v", "video/mpeg");
        AddMimeMapping(".m2a", "audio/mpeg");
        AddMimeMapping(".m2v", "video/mpeg");
        AddMimeMapping(".m3u", "audio/x-mpequrl");
        AddMimeMapping(".m4a", "audio/m4a");
        AddMimeMapping(".m4a", "audio/x-m4a");
        AddMimeMapping(".m4v", "video/mp4");
        AddMimeMapping(".m4v", "video/mpeg4");
        AddMimeMapping(".m4v", "video/x-m4v");
        AddMimeMapping(".man", "application/x-troff-man");
        AddMimeMapping(".manifest", "application/x-ms-manifest");
        AddMimeMapping(".map", "application/x-navimap");
        AddMimeMapping(".mar", "text/plain");
        AddMimeMapping(".mbd", "application/mbedlet");
        AddMimeMapping(".mdb", "application/x-msaccess");
        AddMimeMapping(".mc$", "application/x-magic-cap-package-1.0");
        AddMimeMapping(".mcd", "application/mcad");
        AddMimeMapping(".mcd", "application/x-mathcad");
        AddMimeMapping(".mcf", "image/vasa");
        AddMimeMapping(".mcf", "text/mcf");
        AddMimeMapping(".mcp", "application/netmc");
        AddMimeMapping(".me", "application/x-troff-me");
        AddMimeMapping(".mht", "message/rfc822");
        AddMimeMapping(".mhtml", "message/rfc822");
        AddMimeMapping(".mid", "application/x-midi");
        AddMimeMapping(".mid", "audio/midi");
        AddMimeMapping(".mid", "audio/x-mid");
        AddMimeMapping(".mid", "audio/x-midi");
        AddMimeMapping(".mid", "music/crescendo");
        AddMimeMapping(".mid", "x-music/x-midi");
        AddMimeMapping(".midi", "application/x-midi");
        AddMimeMapping(".midi", "audio/midi");
        AddMimeMapping(".midi", "audio/x-mid");
        AddMimeMapping(".midi", "audio/x-midi");
        AddMimeMapping(".midi", "music/crescendo");
        AddMimeMapping(".midi", "x-music/x-midi");
        AddMimeMapping(".mif", "application/x-frame");
        AddMimeMapping(".mif", "application/x-mif");
        AddMimeMapping(".mime", "message/rfc822");
        AddMimeMapping(".mime", "www/mime");
        AddMimeMapping(".mjf", "audio/x-vnd.audioexplosion.mjuicemediafile");
        AddMimeMapping(".mjpg", "video/x-motion-jpeg");
        AddMimeMapping(".mm", "application/base64");
        AddMimeMapping(".mm", "application/x-meme");
        AddMimeMapping(".mme", "application/base64");
        AddMimeMapping(".mny", "application/x-msmoney");
        AddMimeMapping(".mod", "audio/mod");
        AddMimeMapping(".mod", "audio/x-mod");
        AddMimeMapping(".moov", "video/quicktime");
        AddMimeMapping(".mov", "video/quicktime");
        AddMimeMapping(".movie", "video/x-sgi-movie");
        AddMimeMapping(".mp2", "audio/mpeg");
        AddMimeMapping(".mp2", "audio/x-mpeg");
        AddMimeMapping(".mp2", "video/mpeg");
        AddMimeMapping(".mp2", "video/x-mpeg");
        AddMimeMapping(".mp2", "video/x-mpeq2a");
        AddMimeMapping(".mp3", "audio/mpeg3");
        AddMimeMapping(".mp3", "audio/x-mpeg-3");
        AddMimeMapping(".mp3", "video/mpeg");
        AddMimeMapping(".mp3", "video/x-mpeg");
        AddMimeMapping(".mp4", "video/mp4");
        AddMimeMapping(".mpa", "audio/mpeg");
        AddMimeMapping(".mpa", "video/mpeg");
        AddMimeMapping(".mpc", "application/x-project");
        AddMimeMapping(".mpe", "video/mpeg");
        AddMimeMapping(".mpeg", "video/mpeg");
        AddMimeMapping(".mpg", "audio/mpeg");
        AddMimeMapping(".mpg", "video/mpeg");
        AddMimeMapping(".mpga", "audio/mpeg");
        AddMimeMapping(".mpp", "application/vnd.ms-project");
        AddMimeMapping(".mpt", "application/x-project");
        AddMimeMapping(".mpv", "application/x-project");
        AddMimeMapping(".mpv2", "video/mpeg");
        AddMimeMapping(".mpx", "application/x-project");
        AddMimeMapping(".mrc", "application/marc");
        AddMimeMapping(".ms", "application/x-troff-ms");
        AddMimeMapping(".mv", "video/x-sgi-movie");
        AddMimeMapping(".mvb", "application/x-msmediaview");
        AddMimeMapping(".my", "audio/make");
        AddMimeMapping(".mzz", "application/x-vnd.audioexplosion.mzz");
        AddMimeMapping(".nap", "image/naplps");
        AddMimeMapping(".naplps", "image/naplps");
        AddMimeMapping(".nc", "application/x-netcdf");
        AddMimeMapping(".ncm", "application/vnd.nokia.configuration-message");
        AddMimeMapping(".nif", "image/x-niff");
        AddMimeMapping(".niff", "image/x-niff");
        AddMimeMapping(".nix", "application/x-mix-transfer");
        AddMimeMapping(".nsc", "application/x-conference");
        AddMimeMapping(".nvd", "application/x-navidoc");
        AddMimeMapping(".nws", "message/rfc822");
        AddMimeMapping(".o", "application/octet-stream");
        AddMimeMapping(".oda", "application/oda");
        AddMimeMapping(".odp", "application/vnd.oasis.opendocument.presentation");
        AddMimeMapping(".ods", "application/vnd.oasis.opendocument.spreadsheet");
        AddMimeMapping(".odt", "application/vnd.oasis.opendocument.text");
        AddMimeMapping(".oform", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        AddMimeMapping(".oform", "application/vnd.openxmlformats-officedocument.wordprocessingml.document.oform");
        AddMimeMapping(".oga", "audio/ogg");
        AddMimeMapping(".ogg", "video/ogg");
        AddMimeMapping(".ogg", "audio/ogg");
        AddMimeMapping(".ogv", "video/ogg");
        AddMimeMapping(".omc", "application/x-omc");
        AddMimeMapping(".omcd", "application/x-omcdatamaker");
        AddMimeMapping(".omcr", "application/x-omcregerator");
        AddMimeMapping(".otp", "application/vnd.oasis.opendocument.presentation-template");
        AddMimeMapping(".ots", "application/vnd.oasis.opendocument.spreadsheet-template");
        AddMimeMapping(".ott", "application/vnd.oasis.opendocument.text-template");
        AddMimeMapping(".p", "text/x-pascal");
        AddMimeMapping(".p10", "application/pkcs10");
        AddMimeMapping(".p10", "application/x-pkcs10");
        AddMimeMapping(".p12", "application/pkcs-12");
        AddMimeMapping(".p12", "application/x-pkcs12");
        AddMimeMapping(".p7a", "application/x-pkcs7-signature");
        AddMimeMapping(".p7b", "application/x-pkcs7-certificates");
        AddMimeMapping(".p7c", "application/pkcs7-mime");
        AddMimeMapping(".p7c", "application/x-pkcs7-mime");
        AddMimeMapping(".p7m", "application/pkcs7-mime");
        AddMimeMapping(".p7m", "application/x-pkcs7-mime");
        AddMimeMapping(".p7r", "application/x-pkcs7-certreqresp");
        AddMimeMapping(".p7s", "application/pkcs7-signature");
        AddMimeMapping(".part", "application/pro_eng");
        AddMimeMapping(".pas", "text/pascal");
        AddMimeMapping(".pbm", "image/x-portable-bitmap");
        AddMimeMapping(".pcl", "application/vnd.hp-pcl");
        AddMimeMapping(".pcl", "application/x-pcl");
        AddMimeMapping(".pct", "image/x-pict");
        AddMimeMapping(".pcx", "image/x-pcx");
        AddMimeMapping(".pdb", "chemical/x-pdb");
        AddMimeMapping(".pdf", "application/pdf");
        AddMimeMapping(".pfunk", "audio/make");
        AddMimeMapping(".pfunk", "audio/make.my.funk");
        AddMimeMapping(".pfx", "application/x-pkcs12");
        AddMimeMapping(".pgm", "image/x-portable-graymap");
        AddMimeMapping(".pgm", "image/x-portable-greymap");
        AddMimeMapping(".pic", "image/pict");
        AddMimeMapping(".pict", "image/pict");
        AddMimeMapping(".pkg", "application/x-newton-compatible-pkg");
        AddMimeMapping(".pko", "application/vnd.ms-pki.pko");
        AddMimeMapping(".pko", "application/vndms-pkipko");
        AddMimeMapping(".pl", "text/plain");
        AddMimeMapping(".pl", "text/x-script.perl");
        AddMimeMapping(".plx", "application/x-pixclscript");
        AddMimeMapping(".pm", "image/x-xpixmap");
        AddMimeMapping(".pm", "text/x-script.perl-module");
        AddMimeMapping(".pm4", "application/x-pagemaker");
        AddMimeMapping(".pm5", "application/x-pagemaker");
        AddMimeMapping(".pma", "application/x-perfmon");
        AddMimeMapping(".pmc", "application/x-perfmon");
        AddMimeMapping(".pml", "application/x-perfmon");
        AddMimeMapping(".pmr", "application/x-perfmon");
        AddMimeMapping(".pmw", "application/x-perfmon");
        AddMimeMapping(".png", "image/png");
        AddMimeMapping(".pnm", "application/x-portable-anymap");
        AddMimeMapping(".pnm", "image/x-portable-anymap");
        AddMimeMapping(".pot", "application/mspowerpoint");
        AddMimeMapping(".pot", "application/vnd.ms-powerpoint");
        AddMimeMapping(".potm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
        AddMimeMapping(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
        AddMimeMapping(".pov", "model/x-pov");
        AddMimeMapping(".ppa", "application/vnd.ms-powerpoint");
        AddMimeMapping(".ppm", "image/x-portable-pixmap");
        AddMimeMapping(".pps", "application/mspowerpoint");
        AddMimeMapping(".pps", "application/vnd.ms-powerpoint");
        AddMimeMapping(".ppsm", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
        AddMimeMapping(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
        AddMimeMapping(".ppt", "application/vnd.ms-powerpoint");
        AddMimeMapping(".ppt", "application/mspowerpoint");
        AddMimeMapping(".ppt", "application/powerpoint");
        AddMimeMapping(".ppt", "application/x-mspowerpoint");
        AddMimeMapping(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
        AddMimeMapping(".pptt", "application/pptt");
        AddMimeMapping(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        AddMimeMapping(".ppz", "application/mspowerpoint");
        AddMimeMapping(".pre", "application/x-freelance");
        AddMimeMapping(".prf", "application/pics-rules");
        AddMimeMapping(".prt", "application/pro_eng");
        AddMimeMapping(".ps", "application/postscript");
        AddMimeMapping(".psd", "application/octet-stream");
        AddMimeMapping(".pub", "application/x-mspublisher");
        AddMimeMapping(".pvu", "paleovu/x-pv");
        AddMimeMapping(".pwz", "application/vnd.ms-powerpoint");
        AddMimeMapping(".py", "text/x-script.phyton");
        AddMimeMapping(".pyc", "applicaiton/x-bytecode.python");
        AddMimeMapping(".qcp", "audio/vnd.qcelp");
        AddMimeMapping(".qd3", "x-world/x-3dmf");
        AddMimeMapping(".qd3d", "x-world/x-3dmf");
        AddMimeMapping(".qif", "image/x-quicktime");
        AddMimeMapping(".qt", "video/quicktime");
        AddMimeMapping(".qtc", "video/x-qtc");
        AddMimeMapping(".qti", "image/x-quicktime");
        AddMimeMapping(".qtif", "image/x-quicktime");
        AddMimeMapping(".ra", "audio/x-pn-realaudio");
        AddMimeMapping(".ra", "audio/x-pn-realaudio-plugin");
        AddMimeMapping(".ra", "audio/x-realaudio");
        AddMimeMapping(".ram", "audio/x-pn-realaudio");
        AddMimeMapping(".ras", "application/x-cmu-raster");
        AddMimeMapping(".ras", "image/cmu-raster");
        AddMimeMapping(".ras", "image/x-cmu-raster");
        AddMimeMapping(".rast", "image/cmu-raster");
        AddMimeMapping(".rexx", "text/x-script.rexx");
        AddMimeMapping(".rf", "image/vnd.rn-realflash");
        AddMimeMapping(".rgb", "image/x-rgb");
        AddMimeMapping(".rm", "application/vnd.rn-realmedia");
        AddMimeMapping(".rm", "audio/x-pn-realaudio");
        AddMimeMapping(".rmi", "audio/mid");
        AddMimeMapping(".rmm", "audio/x-pn-realaudio");
        AddMimeMapping(".rmp", "audio/x-pn-realaudio");
        AddMimeMapping(".rmp", "audio/x-pn-realaudio-plugin");
        AddMimeMapping(".rng", "application/ringing-tones");
        AddMimeMapping(".rng", "application/vnd.nokia.ringing-tone");
        AddMimeMapping(".rnx", "application/vnd.rn-realplayer");
        AddMimeMapping(".roff", "application/x-troff");
        AddMimeMapping(".rp", "image/vnd.rn-realpix");
        AddMimeMapping(".rpm", "audio/x-pn-realaudio-plugin");
        AddMimeMapping(".rt", "text/richtext");
        AddMimeMapping(".rt", "text/vnd.rn-realtext");
        AddMimeMapping(".rtf", "application/rtf");
        AddMimeMapping(".rtf", "application/x-rtf");
        AddMimeMapping(".rtf", "text/richtext");
        AddMimeMapping(".rtx", "application/rtf");
        AddMimeMapping(".rtx", "text/richtext");
        AddMimeMapping(".rv", "video/vnd.rn-realvideo");
        AddMimeMapping(".s", "text/x-asm");
        AddMimeMapping(".s3m", "audio/s3m");
        AddMimeMapping(".saveme", "application/octet-stream");
        AddMimeMapping(".sbk", "application/x-tbook");
        AddMimeMapping(".scd", "application/x-msschedule");
        AddMimeMapping(".scm", "application/x-lotusscreencam");
        AddMimeMapping(".scm", "text/x-script.guile");
        AddMimeMapping(".scm", "text/x-script.scheme");
        AddMimeMapping(".scm", "video/x-scm");
        AddMimeMapping(".sct", "text/scriptlet");
        AddMimeMapping(".sdml", "text/plain");
        AddMimeMapping(".sdp", "application/sdp");
        AddMimeMapping(".sdp", "application/x-sdp");
        AddMimeMapping(".sdr", "application/sounder");
        AddMimeMapping(".sea", "application/sea");
        AddMimeMapping(".sea", "application/x-sea");
        AddMimeMapping(".set", "application/set");
        AddMimeMapping(".setpay", "application/set-payment-initiation");
        AddMimeMapping(".setreg", "application/set-registration-initiation");
        AddMimeMapping(".sgm", "text/sgml");
        AddMimeMapping(".sgm", "text/x-sgml");
        AddMimeMapping(".sgml", "text/sgml");
        AddMimeMapping(".sgml", "text/x-sgml");
        AddMimeMapping(".sh", "application/x-bsh");
        AddMimeMapping(".sh", "application/x-sh");
        AddMimeMapping(".sh", "application/x-shar");
        AddMimeMapping(".sh", "text/x-script.sh");
        AddMimeMapping(".shar", "application/x-bsh");
        AddMimeMapping(".shar", "application/x-shar");
        AddMimeMapping(".shtml", "text/html");
        AddMimeMapping(".shtml", "text/x-server-parsed-html");
        AddMimeMapping(".sid", "audio/x-psid");
        AddMimeMapping(".sit", "application/x-sit");
        AddMimeMapping(".sit", "application/x-stuffit");
        AddMimeMapping(".skd", "application/x-koan");
        AddMimeMapping(".skm", "application/x-koan");
        AddMimeMapping(".skp", "application/x-koan");
        AddMimeMapping(".skt", "application/x-koan");
        AddMimeMapping(".sl", "application/x-seelogo");
        AddMimeMapping(".smi", "application/smil");
        AddMimeMapping(".smil", "application/smil");
        AddMimeMapping(".snd", "audio/basic");
        AddMimeMapping(".snd", "audio/x-adpcm");
        AddMimeMapping(".sol", "application/solids");
        AddMimeMapping(".spc", "application/x-pkcs7-certificates");
        AddMimeMapping(".spc", "text/x-speech");
        AddMimeMapping(".spl", "application/futuresplash");
        AddMimeMapping(".spr", "application/x-sprite");
        AddMimeMapping(".sprite", "application/x-sprite");
        AddMimeMapping(".src", "application/x-wais-source");
        AddMimeMapping(".ssi", "text/x-server-parsed-html");
        AddMimeMapping(".ssm", "application/streamingmedia");
        AddMimeMapping(".sst", "application/vnd.ms-pki.certstore");
        AddMimeMapping(".sst", "application/vndms-pkicertstore");
        AddMimeMapping(".step", "application/step");
        AddMimeMapping(".stl", "application/sla");
        AddMimeMapping(".stl", "application/vnd.ms-pki.stl");
        AddMimeMapping(".stl", "application/x-navistyle");
        AddMimeMapping(".stl", "application/vndms-pkistl");
        AddMimeMapping(".stm", "text/html");
        AddMimeMapping(".stp", "application/step");
        AddMimeMapping(".sv4cpio", "application/x-sv4cpio");
        AddMimeMapping(".sv4crc", "application/x-sv4crc");
        AddMimeMapping(".svf", "image/vnd.dwg");
        AddMimeMapping(".svf", "image/x-dwg");
        AddMimeMapping(".svr", "application/x-world");
        AddMimeMapping(".svr", "x-world/x-svr");
        AddMimeMapping(".svg", "image/svg+xml");
        AddMimeMapping(".svgt", "image/svg+xml");
        AddMimeMapping(".swf", "application/x-shockwave-flash");
        AddMimeMapping(".t", "application/x-troff");
        AddMimeMapping(".talk", "text/x-speech");
        AddMimeMapping(".tar", "application/x-tar");
        AddMimeMapping(".tbk", "application/toolbook");
        AddMimeMapping(".tbk", "application/x-tbook");
        AddMimeMapping(".tcl", "application/x-tcl");
        AddMimeMapping(".tcl", "text/x-script.tcl");
        AddMimeMapping(".tcsh", "text/x-script.tcsh");
        AddMimeMapping(".tex", "application/x-tex");
        AddMimeMapping(".texi", "application/x-texinfo");
        AddMimeMapping(".texinfo", "application/x-texinfo");
        AddMimeMapping(".text", "application/plain");
        AddMimeMapping(".text", "text/plain");
        AddMimeMapping(".tgz", "application/x-compressed");
        AddMimeMapping(".tgz", "application/gnutar");
        AddMimeMapping(".tif", "image/tiff");
        AddMimeMapping(".tif", "image/x-tiff");
        AddMimeMapping(".tiff", "image/tiff");
        AddMimeMapping(".tiff", "image/x-tiff");
        AddMimeMapping(".tr", "application/x-troff");
        AddMimeMapping(".trm", "application/x-msterminal");
        AddMimeMapping(".tsi", "audio/tsp-audio");
        AddMimeMapping(".tsp", "application/dsptype");
        AddMimeMapping(".tsp", "audio/tsplayer");
        AddMimeMapping(".tsv", "text/tab-separated-values");
        AddMimeMapping(".turbot", "image/florian");
        AddMimeMapping(".txt", "text/plain");
        AddMimeMapping(".uil", "text/x-uil");
        AddMimeMapping(".uls", "text/iuls");
        AddMimeMapping(".uni", "text/uri-list");
        AddMimeMapping(".unis", "text/uri-list");
        AddMimeMapping(".unv", "application/i-deas");
        AddMimeMapping(".uri", "text/uri-list");
        AddMimeMapping(".uris", "text/uri-list");
        AddMimeMapping(".ustar", "application/x-ustar");
        AddMimeMapping(".ustar", "multipart/x-ustar");
        AddMimeMapping(".uu", "application/octet-stream");
        AddMimeMapping(".uu", "text/x-uuencode");
        AddMimeMapping(".uue", "text/x-uuencode");
        AddMimeMapping(".vcd", "application/x-cdlink");
        AddMimeMapping(".vcf", "text/x-vcard");
        AddMimeMapping(".vcs", "text/x-vcalendar");
        AddMimeMapping(".vda", "application/vda");
        AddMimeMapping(".vdo", "video/vdo");
        AddMimeMapping(".vew", "application/groupwise");
        AddMimeMapping(".viv", "video/vivo");
        AddMimeMapping(".viv", "video/vnd.vivo");
        AddMimeMapping(".vivo", "video/vivo");
        AddMimeMapping(".vivo", "video/vnd.vivo");
        AddMimeMapping(".vmd", "application/vocaltec-media-desc");
        AddMimeMapping(".vmf", "application/vocaltec-media-file");
        AddMimeMapping(".voc", "audio/voc");
        AddMimeMapping(".voc", "audio/x-voc");
        AddMimeMapping(".vos", "video/vosaic");
        AddMimeMapping(".vox", "audio/voxware");
        AddMimeMapping(".vqe", "audio/x-twinvq-plugin");
        AddMimeMapping(".vqf", "audio/x-twinvq");
        AddMimeMapping(".vql", "audio/x-twinvq-plugin");
        AddMimeMapping(".vrml", "application/x-vrml");
        AddMimeMapping(".vrml", "model/vrml");
        AddMimeMapping(".vrml", "x-world/x-vrml");
        AddMimeMapping(".vrt", "x-world/x-vrt");
        AddMimeMapping(".vsd", "application/x-visio");
        AddMimeMapping(".vst", "application/x-visio");
        AddMimeMapping(".vsw", "application/x-visio");
        AddMimeMapping(".w60", "application/wordperfect6.0");
        AddMimeMapping(".w61", "application/wordperfect6.1");
        AddMimeMapping(".w6w", "application/msword");
        AddMimeMapping(".wav", "audio/wav");
        AddMimeMapping(".wav", "audio/x-wav");
        AddMimeMapping(".wb1", "application/x-qpro");
        AddMimeMapping(".wbmp", "image/vnd.wap.wbmp");
        AddMimeMapping(".wcm", "application/vnd.ms-works");
        AddMimeMapping(".wdb", "application/vnd.ms-works");
        AddMimeMapping(".web", "application/vnd.xara");
        AddMimeMapping(".webm", "video/webm");
        AddMimeMapping(".webp", "image/webp");
        AddMimeMapping(".wiz", "application/msword");
        AddMimeMapping(".wk1", "application/x-123");
        AddMimeMapping(".wks", "application/vnd.ms-works");
        AddMimeMapping(".wmf", "windows/metafile");
        AddMimeMapping(".wmf", "application/x-msmetafile");
        AddMimeMapping(".wml", "text/vnd.wap.wml");
        AddMimeMapping(".wmlc", "application/vnd.wap.wmlc");
        AddMimeMapping(".wmls", "text/vnd.wap.wmlscript");
        AddMimeMapping(".wmlsc", "application/vnd.wap.wmlscriptc");
        AddMimeMapping(".woff2", "application/font-woff2");
        AddMimeMapping(".word", "application/msword");
        AddMimeMapping(".wp", "application/wordperfect");
        AddMimeMapping(".wp5", "application/wordperfect");
        AddMimeMapping(".wp5", "application/wordperfect6.0");
        AddMimeMapping(".wp6", "application/wordperfect");
        AddMimeMapping(".wpd", "application/wordperfect");
        AddMimeMapping(".wpd", "application/x-wpwin");
        AddMimeMapping(".wps", "application/vnd.ms-works");
        AddMimeMapping(".wq1", "application/x-lotus");
        AddMimeMapping(".wri", "application/mswrite");
        AddMimeMapping(".wri", "application/x-wri");
        AddMimeMapping(".wri", "application/x-mswrite");
        AddMimeMapping(".wrl", "application/x-world");
        AddMimeMapping(".wrl", "model/vrml");
        AddMimeMapping(".wrl", "x-world/x-vrml");
        AddMimeMapping(".wrz", "model/vrml");
        AddMimeMapping(".wrz", "x-world/x-vrml");
        AddMimeMapping(".wsc", "text/scriplet");
        AddMimeMapping(".wsdl", "text/xml");
        AddMimeMapping(".wsrc", "application/x-wais-source");
        AddMimeMapping(".wtk", "application/x-wintalk");
        AddMimeMapping(".x-png", "image/png");
        AddMimeMapping(".xaf", "x-world/x-vrml");
        AddMimeMapping(".xbm", "image/x-xbitmap");
        AddMimeMapping(".xbm", "image/x-xbm");
        AddMimeMapping(".xbm", "image/xbm");
        AddMimeMapping(".xdr", "video/x-amt-demorun");
        AddMimeMapping(".xgz", "xgl/drawing");
        AddMimeMapping(".xif", "image/vnd.xiff");
        AddMimeMapping(".xl", "application/excel");
        AddMimeMapping(".xla", "application/excel");
        AddMimeMapping(".xla", "application/x-excel");
        AddMimeMapping(".xla", "application/x-msexcel");
        AddMimeMapping(".xla", "application/vnd.ms-excel");
        AddMimeMapping(".xlb", "application/excel");
        AddMimeMapping(".xlb", "application/vnd.ms-excel");
        AddMimeMapping(".xlb", "application/x-excel");
        AddMimeMapping(".xlc", "application/excel");
        AddMimeMapping(".xlc", "application/vnd.ms-excel");
        AddMimeMapping(".xlc", "application/x-excel");
        AddMimeMapping(".xld", "application/excel");
        AddMimeMapping(".xld", "application/x-excel");
        AddMimeMapping(".xlk", "application/excel");
        AddMimeMapping(".xlk", "application/x-excel");
        AddMimeMapping(".xll", "application/excel");
        AddMimeMapping(".xll", "application/vnd.ms-excel");
        AddMimeMapping(".xll", "application/x-excel");
        AddMimeMapping(".xlm", "application/excel");
        AddMimeMapping(".xlm", "application/vnd.ms-excel");
        AddMimeMapping(".xlm", "application/x-excel");
        AddMimeMapping(".xls", "application/vnd.ms-excel");
        AddMimeMapping(".xls", "application/excel");
        AddMimeMapping(".xls", "application/x-excel");
        AddMimeMapping(".xls", "application/x-msexcel");
        AddMimeMapping(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
        AddMimeMapping(".xlsm", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        AddMimeMapping(".xlst", "application/xlst");
        AddMimeMapping(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        AddMimeMapping(".xlt", "application/excel");
        AddMimeMapping(".xlt", "application/x-excel");
        AddMimeMapping(".xlt", "application/vnd.ms-excel");
        AddMimeMapping(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
        AddMimeMapping(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
        AddMimeMapping(".xlv", "application/excel");
        AddMimeMapping(".xlv", "application/x-excel");
        AddMimeMapping(".xlw", "application/excel");
        AddMimeMapping(".xlw", "application/vnd.ms-excel");
        AddMimeMapping(".xlw", "application/x-excel");
        AddMimeMapping(".xlw", "application/x-msexcel");
        AddMimeMapping(".xm", "audio/xm");
        AddMimeMapping(".xml", "application/xml");
        AddMimeMapping(".xml", "text/xml");
        AddMimeMapping(".xmz", "xgl/movie");
        AddMimeMapping(".xof", "x-world/x-vrml");
        AddMimeMapping(".xpix", "application/x-vnd.ls-xpix");
        AddMimeMapping(".xpm", "image/x-xpixmap");
        AddMimeMapping(".xpm", "image/xpm");
        AddMimeMapping(".xps", "application/vnd.ms-xpsdocument");
        AddMimeMapping(".xsd", "text/xml");
        AddMimeMapping(".xsl", "text/xml");
        AddMimeMapping(".xsr", "video/x-amt-showrun");
        AddMimeMapping(".xwd", "image/x-xwd");
        AddMimeMapping(".xwd", "image/x-xwindowdump");
        AddMimeMapping(".xyz", "chemical/x-pdb");
        AddMimeMapping(".z", "application/x-compress");
        AddMimeMapping(".z", "application/x-compressed");
        AddMimeMapping(".zip", "application/zip");
        AddMimeMapping(".zip", "application/x-compressed");
        AddMimeMapping(".zip", "application/x-zip-compressed");
        AddMimeSynonym("application/x-zip-compressed", "application/zip");
        AddMimeMapping(".zip", "multipart/x-zip");
        AddMimeMapping(".zoo", "application/octet-stream");
        AddMimeMapping(".zsh", "text/x-script.zsh");
        AddMimeMapping(".*", "application/octet-stream");
    }

    public static string GetExtention(string mimeMapping)
    {
        if (string.IsNullOrEmpty(mimeMapping))
        {
            return null;
        }

        foreach (DictionaryEntry entry in _extensionToMimeMappingTable)
        {
            var mime = (string)entry.Value;
            if (mime.Equals(mimeMapping, StringComparison.OrdinalIgnoreCase))
            {
                return (string)entry.Key;
            }

            if (!_mimeSynonyms.ContainsKey(mime))
            {
                continue;
            }

            if (_mimeSynonyms[mime].Contains(mimeMapping.ToLowerInvariant()))
            {
                return (string)entry.Key;
            }
        }

        return null;
    }

    public static string GetMimeMapping(string fileName)
    {
        string str = null;
        var startIndex = fileName.LastIndexOf('.');

        if (0 <= startIndex && fileName.LastIndexOf('\\') < startIndex)
        {
            str = (string)_extensionToMimeMappingTable[fileName.Substring(startIndex)];
        }

        if (str == null)
        {
            str = (string)_extensionToMimeMappingTable[".*"];
        }

        return str;
    }

    private static void AddMimeMapping(string extension, string MimeType)
    {
        if (_extensionToMimeMappingTable.ContainsKey(extension))
        {
            AddMimeSynonym((string)_extensionToMimeMappingTable[extension], MimeType);
        }
        else
        {
            _extensionToMimeMappingTable.Add(extension, MimeType);
        }
    }

    private static void AddMimeSynonym(string mime, string synonym)
    {
        if (!_mimeSynonyms.ContainsKey(mime))
        {
            _mimeSynonyms[mime] = new List<string>();
        }

        if (!_mimeSynonyms[mime].Contains(synonym))
        {
            _mimeSynonyms[mime].Add(synonym);
        }
    }
}
