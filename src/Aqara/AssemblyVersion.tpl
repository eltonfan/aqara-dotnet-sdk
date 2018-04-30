using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Elton FAN")]
[assembly: AssemblyProduct("RCPlus")]
[assembly: AssemblyCopyright("\u00A9 2017 Elton FAN (eltonfan@live.cn, http://elton.io)")]
[assembly: AssemblyTrademark("ELTON")]

[assembly: AssemblyVersion("$VERSION$.0")]
[assembly: AssemblyFileVersion("$VERSION$.$REV$")]
[assembly: AssemblyInformationalVersion("v$VERSION$ Beta")]
[assembly: AssemblyDefaultAlias("$HASH$ $TIMESTAMP$")]