Program to organize Image based in its metadata.

To use the program:

KiOrganizer.exe action path options

Actions: 
	* OrganizerImages: Put images in Directory based in differents information.
	* RenameFiles: Rename files in a Directory based in differents information.
	* RemoveCache: Remove saved cache (mainly the save calls to OSM).

Path:
	* Path where you have your images.

Options: 
    * Different options for each Action.
	* RemoveCache: Doesn't have actions.
	* OrganizerImages and RenameFiles has the same actions. It use for define the path to save or the file name. In case the file doesn't have the data you aks for it it replace for a default value (Location: NoLocation, Date: NoDate)
		+ City:  City where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM).
		+ Road:  Road where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM).
		+ Village: Village where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM).
		+ Country: Country where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM).
		+ County: County  where the photo was taken (information from LAT/LONG in the photo and mapped throw OSM).
		+ AmenityType:  If the photo was taken in a specific place (like a restaurant or museum) indicate the type.
		+ AmenityName: If the photo was taken in a specific place indicate the place's name (My Restaurant, British museum).
		+ Date: Date when the photo was taken (format: yyyy-MM-dd)
		+ DateTime: Date and Time when the photo was taken (format: yyyy-MM-dd HH_mm_ss) 
		+ Time: Time when the photo was taken (format: HH_mm_ss)
		+ YearMonth: Year and Month when the photo was taken (format: yyyy-MM)
		+ Noop: Do nothing.

Alternatives: 
   In the case the photo doesn't have the information you need, you can specify other parameter as aternative with a backslash. For example: City\County or City\Noop. For the moment the Program file just work with one alternative.
    
   
Example of Uses:

   KiOrganizer.exe OrganizerImages C:\MyPhotos Country\County YearMonth AmenityName\Noop
      Example of folder created:
	       C:\MyPhotos\Organized\Scotland\201810\ImageWithCountryInfo.jpg
		   C:\MyPhotos\Organized\Scotland\201810\Nessy Restaurant\ImageWithCountryInfoAndAmenity.jpg
		   C:\MyPhotos\Organized\City of Edinburgh\201811\ImageWithOnlyCountyInfo.jpg
		   C:\MyPhotos\Organized\NoLocation\201810\Image With no location.jpg
	    