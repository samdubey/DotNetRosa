using System;
namespace org.javarosa.core.reference
{
	
	/// <summary> A ReferenceFactory is responsible for knowing how to derive a 
	/// reference for a range of URI's. ReferenceFactories may or may 
	/// not be present in different environments.
	/// 
	/// ReferenceFactory are not required to generate particular references, and
	/// may rely on (or attempt to rely on) other factories in implementation,
	/// negotiated through the reference manager.
	/// 
	/// In general, simple reference derivations should happen using a
	/// PrefixedRootFactory, which handles most of the URI munging for you
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public interface ReferenceFactory
	{
		bool derives(System.String URI);
		Reference derive(System.String URI);
		Reference derive(System.String URI, System.String context);
	}
}