var gulp = require('gulp');
var concatCss = require('gulp-concat-css');
var minifyCss = require('gulp-minify-css');
var clean = require('gulp-dest-clean');

gulp.task('default', ['copy']);

gulp.task('copy', ['copy:css', 'copy:js', 'copy:fonts']);

gulp.task('copy:css', function () {
    return gulp.src(['./node_modules/bootstrap/dist/css/bootstrap.min.css', './Css/bootstrap.ext.css'])
        .pipe(concatCss("bootstrap.min.css"))
        .pipe(minifyCss())
        .pipe(gulp.dest('./wwwroot/css'))
        .pipe(clean('./wwwroot/css'));
});

gulp.task('copy:fonts', function () {
    return gulp.src('./node_modules/bootstrap/dist/fonts/*').pipe(gulp.dest('./wwwroot/fonts')).pipe(clean('./wwwroot/fonts'));
});

gulp.task('copy:js', function () {
    return gulp.src(['./node_modules/bootstrap/dist/js/*.min.js', './node_modules/jquery/dist/*.min.js'])
            .pipe(gulp.dest('./wwwroot/js')).pipe(clean('./wwwroot/js'));
});
