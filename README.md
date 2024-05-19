Pattern files are located in DirectoryCatalog/Composite and DirectoryCatalog/ChainOfResponsibility folders.
GUI is developed on WPF platform, but part of WPF doesn't have mush value from a code point of view,
because i wasn't interested of creating quality WPF code, but was interested in implementing these patterns
for modeling file catalog.

In brief, Composite pattern responsible for creating tree structure where Directories are branches, and Files are leaves.

And Chain of Responsibility pattern responsible for traversing each element of the structure to find a match based on the name and file type criteria.
