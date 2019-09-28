![Covid-19 and AI](/images/covid-head-inv.jpg =461x230)
*When viral marketing goes too far*

A recent post which claims to detect COVID-19 using a deep neural network with a very high accuracy gains keen mediatic interest.


### AI is wonderful

An Australian PhD candidate in artificial intelligence made a [recent post](https://www.linkedin.com/posts/activity-6645711949554425856-9Dhm/) on LinkedIn about his researches on SARS-CoV-2. The post gathered thousands of views, likes, and shares.

He built a Deep Learning model which is able to predict from chest radiographs with a **97.5% accuracy** whether a patient is infected with the COVID-19 virus or not. 

As it stands, the project features:
 - A trained PyTorch model
 - Containerized applicative code
 - A GitHub [repository](https://github.com/elcronos/COVID-19) translated in 8 languages
 - A web application in development
 - A mobile application in development
 - Blueprints for a serverless architecture in AWS to host the model
 - Lots of efforts going on marketing and sponsorship

Everything above was built in one rough week. 

### Deep learning approach in medical diagnosis

Deep convolutional networks have potential benefits towards disease diagnosis and treatment. Many scientific publications have emerged in the recent years [1], here are a few of them:

- In 2016, a group of London researchers published a method for diagnosis of diabetic retinopathy with 86% accuracy, trained on a dataset of 80,000 fundus photographs [2].

- In the same year, Ugandan researchers evaluated the performance of CNNs on microscopic blood smears using a dataset of 10,000 objects [3].

- An effort to classify lung nodules was led by two Japanese researchers on a dataset of 550,000 CT scans [4].

But here, a quick glimpse into the GitHub repository depicts at best an acute lack of understanding in deep learning and AI, and at worst a vicious attempt at getting self-promotion while capitalizing on the pandemic. Here is why.

## Bad engineering

The latent neural representation of these networks is very complex, hence they require a lot of training samples, as depicted in the aforementioned studies.

**As of now, the COVID-19 detector was trained on a dataset of... 30 images!**

![Covid-19 and AI](/images/chest-xray.jpg =560x315)
*Fig. 1: The model learns from chest X-Ray*

For a network that has more than 150 layers and over 20 million parameters, this is completly absurd. This approach was debunked in the following [Reddit thread](https://www.reddit.com/r/MachineLearning/comments/fni5ow/d_why_is_the_ai_hype_absolutely_bonkers/).

Moreover, there is a huge data bias. The 30 pictures are not labeled based on whether an individual had the virus or not, they are labeled based on lung damage for acute cases of COVID-19. Unless your lungs are already wrecked by the virus, the model has no way to detect an infection. In the case where a person presents symptoms of pneumonia, if those symptoms are not acute the accuracy of this model is unproven.

Finally, the COVID model is based on a popular baseline network, the ResNet-50[5]. While this an usual approach for image recognition and classification, the ResNet was pre-trained using photos of everyday objects. Thus, the internal representation of its hidden layers is activated with geometrical forms and colorful patterns (2).

![Visualization of ResNet features](/images/resnet-viz.png =650x160)
*Fig. 2: Visualization of ResNet features [6]*

Such patterns are nowhere to be found in radiographs. This is why most of the medical neural networks are made from scratch.

Many other problems appear when we take a closer look at the code repository. Training, validation and testing datasets contain duplicate images; most of the training process has been taken from a PyTorch tutorial obfuscated with unnecessary code; the Github issues are ridiculous...


**Clearly, that post was destined for thousands of likes, shares and views, no matter what the content behind the title was.**


### Snake oil from a Nigerian prince

<center><i><h4>Propaganda as an instrument of commercial competition has opened opportunities to the inventor and given great stimulus to the research scientist.</i> <br/><br/>Edward L. Bernays</h4></center><br/><br/>

Yet, the author doesn't despair when faced the truth, and often comes forward with the following answer:

«*Hi xxx, we have curated 5000 with the support of radiologist from a Research Institute in Canada* »

I don't know the part of truth in this bold answer, but if such a model is used as-is for a medical application, it can be very dangerous.

The author even created a [Slack group](https://join.slack.com/t/covid-19detector/shared_invite/zt-cw28jq9g-2FcPBD~zNRYLnVhr34hrRQ) with multiple channels. Needless to say, it gathered a lot of interest.

The *#datascientists* channel doesn't have a lot of serious content and is punctuated by enthusiastic newcomers with a lot of hope but very little experience. Similarly,  the only tangible content in the *#doctors* channel comes from professionalS addressing medical issues, for instance that chest X-Rays isn't the recommanded approach for a COVID-19 diagnosis. Finally, the *#researchers* channel is almost empty.

On the other hand, the UI/UX channels are generating a lot of content. The initiative now has 5 different logos, and a mockup for both a mobile and a web interface.

There is even a *#marketing* channel to find ways to enhance communication and raise funds and a *#sponsors* channels with potential investors asking about future prospects of return on investment.<br><br>
[![Slack group for the project](/images/slack-logo.jpg =374x107)*See by yourself*](https://join.slack.com/t/covid-19detector/shared_invite/zt-cw28jq9g-2FcPBD~zNRYLnVhr34hrRQ)<br><br>

## Beware of the hype

**Deep learning is not a silver bullet solution**. Many unprepared companies who tried to internalize it into data squads went nuts after they saw their cost rising while little or nothing was going into production.

Still, advances in AI are groundbreaking nowadays. One would be crazy to ignore them completly.

That doesn't mean diving straight into the pool and flailing around in the water
gasping for some air. Hence the importance of a rock solid team with *transversal skills* in AI / ML, DataOps, architecture, development and many others topics.


### References 

[1] Deep Learning for Medical Image Processing: Overview, Challenges and Future, Muhammad Imran Razzak, Saeeda Naz, Ahmad Zaib, 2017.

[2] Harry Pratt, Frans Coenen, Deborah M Broadbent, Simon P Harding,
and Yalin Zheng. Convolutional neural networks for diabetic retinopathy, 2016.

[3] John A Quinn, Rose Nakasi, Pius KB Mugagga, Patrick Byanyima,
William Lubega, and Alfred Andama. Deep convolutional neural networks for microscopy-based point of care diagnostics, 2016.

[4] Masaharu Sakamoto and Hiroki Nakano. Cascaded neural networks
with selective classifiers and its evaluation using lung x-ray ct images, 2016.

[5] Deep Residual Learning for Image Recognition, Kaiming He Xiangyu Zhang Shaoqing Ren Jian Sun, 2015.

[6] Brian Chu, Daylen Yang, Ravi Tadinada. Visualizing Residual Networks, 2017.


