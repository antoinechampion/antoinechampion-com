##### Industry is a indeed a very harsh market.

It requires significant investments and a lot of R&D to produce small improvements in production. Nonetheless, *a few tenth of a seconds earned on a route sheet can lead to huge gains*. Thus, it is a sector where efficiency must be a huge concern. 
As a consequence, wealthier countries which were formally known as manufacturing powerhouses of the world have been hollowed out by competitors from developing economies.

A new design pattern emerged to get the industry back on the track in these countries. It aims to match the sector with the new digital trends in order to make it fit with our modern society. The whole concept is called **Industry 4.0**.  As of now, it  recently overcame the status of proof of concept, and was adopted by [several](https://www.groupe-psa.com/en/automotive-group/industrial-performance/) [major](https://www.airbus.com/newsroom/press-releases/en/2019/02/airbus-invests-25-million-in-the-future-of-its-aerospace-site-in-ottobrunntaufkirchen-near-munich.html) [groups](https://www.ibm.com/industries/manufacturing/smart-manufacturing-technology).
### About collaborative robots

One of the major innovations of this new trend was about augmented machines called collaborative robots. These machines have 6-axes arms, meaning that they can move to whatever position is within their reach. Moreover, they are made to work **in collaboration with humans**, rather than instead of them, so they are built with lightweight materials and made with rounded edges. If an operator comes to be in contact with the robot, the latter will shutdown instantly for safety.

![A collaborative robot](https://www.robotics.org/userAssets/ogImage/future-robots-2.jpeg =560x400)
*Fig. 1: A collaborative robot - Image credits to Robotic Industries Association.*

What is impressive is that those *ColBots* are not tied to a single task. Depending on what model is fed to them, they can do anything from welding to product conditioning, in order to automate repetitive, unergonomic tasks. The 3 main goals of collaborative robots are:

 - Simplicity: it must be simple to program, to maintain, and to use.
 - Interoperability: it can change its activity quickly.
 - Security: it must be safe enough to work in collaboration with humans.
 
### AI for collaborative robots

Still, the industry have a natural appetite towards efficiency. In contrast to traditional industrial robots which are made to operate quickly with a minimal guidance on a very specific task, they are indeed slower, thus they cannot afford to waste any more second in their decision-making process.

#### Gestures recognition

A lot of progress has been made in the field of computer vision for the industry, for instance about [gestures recognition](https://www.researchgate.net/publication/330429917_Online_Recognition_of_Incomplete_Gesture_Data_to_Interface_Collaborative_Robots) *(O. Gibaru et al., 2019)*. Because ColBots are all about being used in collaboration with an operator, they need to be aware of human gestures at any time. They are cribbled with sensors, and that's a lot of data to work with. Data coming from motion detection is segmented with a genetic algorithm, then reducted before being send to various classifiers for appropriate discrimination.

The aforementioned paper makes us realize that the industry is still all about efficiency and pragmatism: the researchers found out that a standard shallow DNN with more preprocessing is at least as efficient as LSTM/CNN based solutions, so they dove deeper in this direction. 

#### Autonomous sorting

This constant need for efficiency is also depicted on the lastest researches about [objects sorting for the industry](https://www.researchgate.net/publication/324254264_Unsupervised_Robotic_Sorting_Towards_Autonomous_Decision_Making_Robots) *(J. Guérin et al., 2018)*. Unsupervised image classification by features extraction is a domain of computer vision with a lot of publications, and subsequently where a lot of progress has been made through the past 10 years. Unsupervised clustering has evolved and is very efficient when used in pair with a pre-trained network through transfer learning, reducing greatly the degeneracy of the model.

Yet, a market with such important material and human costs can not afford to lose a single bit of accuracy ([or whatever your metric is](https://towardsdatascience.com/accuracy-precision-recall-or-f1-331fb37c5cb9)). You want your model **to be functional all the time no matter the conditions**, for instance whatever the brightness of your plant might be. Here, they provide a way to use a ColBot for a pick-and-place application, using a pre-trained Xception CNN combined with clustering. Thus, those researches are more targeted towards finding the best way to do a simple task, instead of finding a way among others to do a very complicated set of tasks. 

![Sample images from the robustness validation dataset.](https://www.researchgate.net/profile/Joris_Guerin/publication/324254264/figure/fig3/AS:612408993280002@1523021090692/Sample-images-from-the-robustness-validation-dataset.png =587x227)
*Fig. 2: Sample images from the robustness validation dataset - Image credits to J. Guérin et al.*

### Future will be perfect

Collaborative robots are only a part of the whole paradigm. Other fields where artificial intelligence has found its way are, among others, supply chain management, quality control, microelectronics manufacturing, and product distribution. 

Now, there was a noticeable hype around the Industry 4.0 buzzword when it first came out. Still, a sudden hype around an inspiring and promising concept doesn't mean that this very concept is useful. If you are working in data science you should probably be aware of this fact!

Is it really competitive?  Are the new involved business plans reliable? 

And does it makes everyones life better, from the worker to the consumer?

[It sounds](https://www.ey.com/ch/en/newsroom/news-releases/news-release-ey-industry-4-0-is-becoming-increasingly-important-for-swiss-companies-investment-requirements-are-the-greatest-obstacle) [like](https://www2.deloitte.com/us/en/insights/focus/industry-4-0/digital-industrial-transformation-industrial-internet-of-things.html) [a yes](https://www.mckinsey.com/~/media/mckinsey/business%20functions/mckinsey%20digital/our%20insights/getting%20the%20most%20out%20of%20industry%204%200/mckinsey_industry_40_2016.ashx).

### TL;DR

Things are getting promising about AI in the Industry 4.0 paradigm. As of computer vision, it found its use in collaborative robotics.

And while the factory of the future can cope with data analytics, it is not mandated to deal with fancy solutions to put arousing keywords on business presentations. 

The industrial actors have significant financial resources, but are most of all pragmatic. If the Industry 4.0 concept overcame the state of the art, it wasn't because of a marketing hype.




