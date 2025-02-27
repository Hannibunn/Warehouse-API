# vim: noet
.PHONY: _Diplomarbeit.pdf

all: _Diplomarbeit.pdf

_Diplomarbeit.pdf: _Diplomarbeit.tex
#	pdflatex --shell-escape dic3ahel_1.tex
	pdflatex _Diplomarbeit.tex

clean:
	rm -vf  *.bcf *.glo *.idx *.ilg *.ind *.ist *.run.xml
	rm -vf *.out *.log *.dvi *.aux *.cb *.cb? *.toc *~
	rm -Rf _minted*/
