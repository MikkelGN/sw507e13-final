set output "ema.eps"
set term postscript
set title "Estimated Moving Average Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "ema.dat" title "Moving Average" with lines lt 1 lc rgb 'blue', "acceleration.dat" title "Raw Acceleration" with lines lt 1 lc rgb 'red'