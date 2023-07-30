# Slang of the day

Being a non-native English speaker, I often face new, colorful, sometimes very unexpected slang expressions that, to say the least, let me raise an eyebrow. This application just searches [Urban Dictionary](https://www.urbandictionary.com/) for me to enrich my culture.

**Warning:** the output of this application may be not safe for work, and this is out of control of this application.

Given a text file named `terms.txt` containing newline-separated unknown slang expressions (which is your responsibility to obtain), this application chooses one randomly and queries the [Urban Dictionary HTTP API](https://blog-proxy.rapidapi.com/urban-dictionary-api-with-python-php-ruby-javascript-examples/) and displays the list of definitions found, in a [Markdown](https://www.markdownguide.org/basic-syntax/) compatible format.

## Running the application

- Sign up to [Rapid API](https://rapidapi.com/community/api/urban-dictionary) to get API KEY and insert it into the `X-RapidAPI-Key` in the code
- Replace or enrich the `terms.txt` file as you wish
- Ensure that you have the .NET 6 runtime installed and start `dotnet run` from the command line in the source directory

## License

Permissive, [2-clause BSD style](https://opensource.org/license/bsd-2-clause/)