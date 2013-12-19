reset
n=20 #number of intervals
max=5. #max value
min=-165. #min value
width=10#interval width
#function used to map a value to the intervals
hist(x,width)=width*floor(x/width)+width/2.0
set term postscript
set output "barchart.eps"
set xrange [0:*]
set yrange [0:*]
set title "Bar Chart of Frequency"
#to put an empty boundary around the
#data inside an autoscaled graph.
set offset graph 0.05,0.05,0.05,0.0
set boxwidth width*0.9
set style fill solid 0.5 #fillstyle
set tics out nomirror
set xlabel "Percentage"
set grid ytics
set ylabel "Frequency"
#count and plot
plot "bardata.dat" u (hist($1,width)):(1.0) smooth freq w boxes lc rgb "blue" notitle