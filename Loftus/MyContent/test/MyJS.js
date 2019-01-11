var tl2 = new TimelineMax();
var element = document.getElementById('ArtBox');
new ResizeSensor(element, function () {
	if (element.clientHeight > 40) {
		$('#artBeauty').addClass('animated fadeOutDown')
		$('.abMobile').addClass('animated fadeOutUp')

		tl2.to('#artBeauty, .abMobile', 0.5, {
			ease: Power3.easeInOut,
			margin: '0px',
			height: '0px',
			onComplete: function () {
				$('#artBeauty, .abMobile').remove()
			}
		});
	}
});

$(window).resize(function () {
	if (window.innerWidth > 600 && $("#ArtBox").css('display') == "none") {
		$('#titleID').css({ 'margin-bottom': '80px' })
	}
	if (window.innerWidth < 600) {
		$('#titleID').css({
			'margin-bottom': '0px'
		})
	}
});

/* Click name to go back home */
$('#clickme').on('click', function () {
	if ($('#clickme').hasClass('goHome')) {
		$('body').fadeOut(300, function () {
			$('#clickme').removeClass('goHome')
			location.reload();
		})
	}
})



window.addEventListener("orientationchange", function () {
	if (lightbox.currentImageIndex != null) {
		var i = lightbox.currentImageIndex;
		lightbox.changeImage(i);
	}
}, false);



function Cap(string) {
	newString = "";
	var arr = _.words(string);
	_.each(arr, function (w) {
		newString += _.capitalize(w) + " ";
	})
	return newString;
}

document.addEventListener('DOMContentLoaded', function () {
	var elems = document.querySelectorAll('.dropdown-trigger');
	var instances = M.Dropdown.init(elems);
});

$('#ArtBox').hide()
$('#btnDIV').hide()
$('#ddDivArt, #ddDivBeauty').hide()
$('#emailHeader').hide();
$('#makeupBtnGroup').hide();
$('#artBtnGroup').hide();
$('#loadingBox').css('opacity', 0)

$('.btn-secondary').mouseout(function () {
	$('.btn-secondary').removeClass('focus')
})



var animationEnd = (function (el) {
	var animations = {
		animation: 'animationend',
		OAnimation: 'oAnimationEnd',
		MozAnimation: 'mozAnimationEnd',
		WebkitAnimation: 'webkitAnimationEnd',
	};

	for (var t in animations) {
		if (el.style[t] !== undefined) {
			return animations[t];
		}
	}
})(document.createElement('div'));



function rand() {
	var t = Math.random().toString();
	var s = t.replace(".", "");
	return s;
}




$(function () {

	$('#titleID').one(animationEnd, function () {

		var artwork = [];
		var beauty = [];

		$.ajax({
			type: "get",
			url: "/mloftus/GetPics",
			dataType: "json"
		}).done(function (data) {
			_.each(data, function (img) {
				if (_.toLower(img.Category) == "artwork") {
					artwork.push(img)
				}
				else {
					beauty.push(img)
				}
			})
			let artData = _.template($('#photos').html());
			$('#mainGrid').append(artData(artwork));

			let beautyData = _.template($('#photos').html());
			$('#beautyGrid').append(beautyData(beauty));

			//var $grid = $('.grid').isotope({});


			lightbox.option({
				'resizeDuration': 200,
				'imageFadeDuration': 200,
				'fadeDuration': 400,
				'positionFromTop': 50,
				'showImageNumberLabel': false,
				'wrapAround': true
			})
		});
	})
});






function loadPics(elem) {

	var isArt = false;
	var isBeauty = false;
	var width = $('#loadingBox').width()
	$('#loading').height(width)

	if (elem == 'artID') {
		isArt = true;
		$('#beautyGrid').hide()
	}
	if (elem == 'beautyID') {
		isBeauty = true;
		$('#mainGrid').hide()
	}

	$('#loadingBox').animate({ opacity: 1 }, 300);
	$('.selections').animate({ opacity: 0 }, 1000);
	$('#test').css({ 'cursor': 'pointer' })
	var tl = new TimelineMax();

	TweenLite.delayedCall(1, function () {
		$('#ArtBox').show()
		$('#btnDIV').fadeIn(500)

		if (isBeauty) {
			$('#ddDivBeauty').fadeIn(500)
			$('#makeupBtnGroup').show()
		}
		if (isArt) {
			$('#ddDivArt').fadeIn(500)
			$('#artBtnGroup').show()
		}


		var $grid = $(".artGrid").imagesLoaded(function () {
			$grid.isotope({
				itemSelector: '.grid-item',
				transitionDuration: '0.5s',
				stagger: 60,

				masonry: {
					columnWidth: 245,
					isFitWidth: true,
					gutter: 20
				}
			})
		});
		$grid.on('arrangeComplete',
			function (event, laidOutItems) {
				$('#clickme').addClass('goHome')
				$('#mainGrid').animate({ opacity: 1 }, 300);
			}
		)
		var $grid1 = $(".beautyGrid").imagesLoaded(function () {
			$grid1.isotope({
				itemSelector: '.grid-item',
				transitionDuration: '0.5s',
				stagger: 60,

				masonry: {
					columnWidth: 245,
					isFitWidth: true,
					gutter: 20
				}
			})
		});
		$grid1.on('arrangeComplete',
			function (event, laidOutItems) {
				$('#clickme').addClass('goHome')
				$('#mainGrid').animate({ opacity: 1 }, 300);
			}
		)
	})




	if (window.innerWidth > 600) {
		tl.to("#titleID", 0.6, {
			ease: Expo.easeOut,
			margin: '0px auto 30px auto',
			borderColor: "rgba(0,0,0,0)",
			backgroundColor: 'rgba(0, 0, 0, 0)',
			padding: '4px 65px 8px 20px'
		}).to("#titleID", 1, {
			ease: Power3.easeInOut,
			backgroundColor: 'rgba(0, 0, 0, 0)',
			width: '100%'
		}, "-=0.3")
		tl.call(function () {
			$('#titleID').addClass("smallTitle");

		}, null, null, 2);

		setTimeout(function () { $('#emailHeader').fadeIn(600) }, 2500)

	}
	if (window.innerWidth < 600) {
		tl.to("#titleID", 0.6, {
			ease: Expo.easeOut,
			margin: '0px auto 0px auto',
			borderColor: "rgba(0,0,0,0)",
			padding: '0px 0px 0px 0px',
			textAlign: 'center'
		}).to("#titleID", 1, {
			ease: Power3.easeInOut,
			width: '100%'
		}, "-=0.3")
		setTimeout(function () {
			$('#emailHeader').fadeIn(600)
		}, 2500)
	}
};





$(window).resize(function () {
	var width = $('#loadingBox').width()
	$('#loading').height(width)
})

$('.filterButtons').change(function () {
	var $grid = $('.grid').isotope({});
	var filterValue = $(this).attr('data-filter');
	$grid.isotope({ filter: filterValue });
})

$('li a.filterButtons').click(function () {
	var $grid = $('.grid').isotope({});
	var filterValue = $(this).attr('data-filter');
	$grid.isotope({ filter: filterValue });
})

$('li a.filterButtons').click(function () {
	$('.catButton').text(this.innerText)
})
