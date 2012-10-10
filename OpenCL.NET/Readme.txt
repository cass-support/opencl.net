Readme file for OpenCL.NET
------------------------
http://www.cass-hpc.com/solutions/libraries/opencl-net

OpenCL.NET provides access to OpenCL(TM) functionality from .NET based applications
on windows, linux and other supported systems.

OpenCL is a computing language developed by the Khronos Group.
For more information you may take a look at: http://www.khronos.org/opencl

Releases
--------
Release 1.0.48.5:
	* Added extended API for clEnqueueReadBuffer and clEnqueueWriteBuffer.

Release 1.0.48.4:
	* Added support for null events in functions that operate on queues.

Release 1.0.48.3:
	* Added extended API for clSetKernelArg and clCreateProgramWithSource.

Release 1.0.48.2:
	* Fixed clCreateContext* context properties parameter.

Release 1.0.48.1:
	* Fixed 'Vender' to 'Vendor' in CLPlatformInfo enum.

Release 1.0.48:
	* Added support for OpenCL 1.0.48 standard revision.

Release 1.0.43.2:
	* Added SizeT structure for transparent compatibility between 32/64 bit systems.

Release 1.0.43.1:
	* Better support for 32/64 bit platforms.

Release 1.0.43:
	* Various changes to conform to 1.0.43 revision of OpenCL.
	
Release 1.0:
	* Initial implementation of the library, conforms to version 1.0 of OpenCL.
	
License Agreement, rights and privacy
-------------------------------------
OpenCL.NET is provided free of charge, for private, academic, research or commercial use.
The library is provided as is, without warranty or support of any kind.
Please add the relevant details on a credit if possible.
For support or development services: support@cass-hpc.com.

Company details:
----------------
Company for Advanced Supercomputing Solutions Ltd
Bosmat 2a St.
Shoham, Israel 60850
P.O.B 3133
support@cass-hpc.com

All rights reserved (c) 2008 - 2012