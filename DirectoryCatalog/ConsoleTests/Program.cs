using DirectoryCatalog.Composite;

DirectoryItem rootDirectory = new DirectoryItem("C:", null);

DirectoryItem officeInstruments = new DirectoryItem("OfficeInstruments", rootDirectory);
FileItem word = new FileItem("Microsoft Word", officeInstruments, ItemType.Office);
FileItem excel = new FileItem("Microsoft Excel", officeInstruments, ItemType.Office);
officeInstruments.Add(word);
officeInstruments.Add(excel);

rootDirectory.Add(officeInstruments);



DirectoryItem gamingAndMultimedia = new DirectoryItem("Gaming&Multimedia", rootDirectory);
rootDirectory.Add(gamingAndMultimedia);

DirectoryItem games = new DirectoryItem("Games", gamingAndMultimedia);

DirectoryItem multimedia = new DirectoryItem("Multimedia", gamingAndMultimedia);
FileItem myVideo = new FileItem("MyVideo", multimedia, ItemType.Multimedia);

gamingAndMultimedia.Add(games);
gamingAndMultimedia.Add(multimedia);

Console.WriteLine(word.GetPathToCurrent().Item1);
Console.WriteLine(myVideo.GetPathToCurrent().Item1);

Console.ReadLine();