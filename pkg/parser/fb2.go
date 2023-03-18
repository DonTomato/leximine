package parser

import (
	"encoding/xml"
	"io/ioutil"
)

type Fb2 struct {
	XMLName xml.Name `xml:"FictionBook"`
	Body    struct {
		XMLName  xml.Name `xml:"body"`
		Sections []struct {
			XMLName    xml.Name `xml:"section"`
			Title      string   `xml:"title,omitempty"`
			Paragraphs []struct {
				XMLName xml.Name `xml:"p"`
				Text    string   `xml:",chardata"`
			} `xml:"p,omitempty"`
		} `xml:"section,omitempty"`
	} `xml:"body,omitempty"`
}

func ParseFb2(fileName string) (*Fb2, error) {
	fb2, err := ioutil.ReadFile(fileName)
	if err != nil {
		return nil, err
	}
	var f Fb2
	err = xml.Unmarshal(fb2, &f)
	if err != nil {
		return nil, err
	}
	return &f, nil
}
