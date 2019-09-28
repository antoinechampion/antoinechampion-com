<center><h4>The quiet statisticians have changed our world; not by discovering new facts or technical developments, but by changing the ways that we reason, experiment and form our opinions. –Ian Hacking</h4><br/></center>

Given a collection of observations $X$ and expected outputs $Y$, can you write a mapping $X \rightarrow Y_{predicted}$ which would optimally reduce the mean between $Y$ and $Y_{predicted}$? Or the variance?
Can you find the best theoretical predictor which would minimize any given loss function? 

This post will provide answers to these questions by providing an quick introduction to statistical decision theory.

### Statistical Decision Theory

Our model is defined with several assumptions:
- We have an input vector $X$ of $p$ random parameters.
- We have an expected output variable $Y$.
- The goal is to find a predictor $f$ to predict $Y = f(X)$.
- To search for an optimal $f$ given a criteria, we will choose a loss function $L_f$.
- Our loss function will return maximal values if the prediction is far from reality *according to our criteria*.

Since the quality of the predictor is measured by the loss function $L_f$, the optimal predictor $f$ is only optimal in the sense that $L_f$ dictates. Different choices of loss functions lead to different optimal solutions.
It is in general never possible to come up with a predictor that is optimal in every thinkable way. Hence, to choose between a set of predictors we will compare the mean value $E$ of their loss function.

## Search of the best predictor

We denote any probability distribution as $\mathbb{P}$ and the expected value as $E$.

$$
E(L_f(X,Y)) = \int L_f(x,y) \mathbb{P}(x,y)dxdy
$$

From Bayes theorem:
$$
\mathbb{P}(X, Y) = \mathbb{P}(Y|X)\mathbb{P}(X)
$$

Then by splitting the bivariate integral,
$$
E(L_f(X,Y))=E_X E_{Y|X}(L(X,Y)|X)
$$
Now it is possible to find $f$ by minimizing the expected error pointwise.
Let $c \in \mathbb{R}^p$:

$$
f(x) = \text{ argmin}\_c \ E_{Y|X}(L_{Id}(c,Y)|X=x) 
$$

with $L_{Id}$ the loss function applied to the identity function.

A case study will help to clarify how to find $f$ on a given loss function.


## Case study : Mean Squared Error

The most used loss function that appear in ML/DL is often the mean squared error:
$$
L_f(X,Y) = (Y−f(X))^2
$$
From the previous section,
 
<p>
$$
\begin{align*}
E(L_f(X,Y)) & = E(Y−f(X))^2 \\\\
 & = E_X E_{Y|X}([Y−f(X)]^2|X)
\end{align*}
$$
</p>

Hence,

$$f(x) = argmin_c \ E_{Y|X}([Y−c]^2|X=x)$$

The solution is $f(x) = E(Y|X=x)$, because :
$$
E([Y-f(X)]^2|X=x) = V(Y|X=x) + (f(x)−E(Y|X=x))^2
$$

We just proved that when the loss function is the mean squared error, the best predictor of $Y$ at any point $X=x$ is the conditional mean:
$$
f(x) = E(Y|X=x) =\alpha + x^T \beta
$$

This predictor is often referred to as linear regression. It is possible to solve for $\beta$ by injecting $f(x)$ into the integral and equating to zero the vector derivative with respect to $\beta$.

Another possible choice of loss function is the absolute value of the deviation:
$$L_f(X,Y) = |Y−f(X)|$$
One can prove that the optimal predictor is the conditional median:
$$f(x) = median(Y|X=x)$$
Which is a different measure of location, and its estimates are more robust than those for the conditional mean. Unfortunately, absolute value of deviation have discontinuities in their derivatives, which have hindered their widespread use for loss functions.


### Predictor for categorical variables

When we predict a variable Y that take discrete values $c_1, c_2, \ldots, c_k$ which can be regarded as some kind of group labels, we often talk about classification.
Our loss function can be represented by a $K\times K$ matrix $L_f$, with $K$ the number of classes. $L_f(c_i, c_j)$ is the loss for a misclassification of class $j$ instead of class $i$.

<p>
$$
L_f = 
\begin{bmatrix}
L_f(c_1,c_1) = 0 & \cdots & L_f(c_1,c_n)\\
\vdots & \ddots & \vdots \\
L_f(c_n,c_1) & \cdots & L_f(c_n,c_n) = 0
\end{bmatrix}
$$
</p>

Then, we calculate the mean value of our loss function:
$$
E[L_f(Y,f(X))] =E_X \sum_{k=1}^K L_f[c_k,f(X)]\mathbb{P}(c_k|X)
$$
It is now possible to minimize the expectation of the loss pointwise to get an expression for f:
$$
f(x) = argmin_{c = c_1,\ldots, c_K} \ \sum_{k=1}^K L_f(c_k,c)\mathbb{P}(c_k|X=x)
$$

## Case study : Bayes classifier
A very common case of loss function is the zero-one loss. If the class predicate is right, loss is 0. If it is wrong, loss is 1:
<p>
$$
L_f =
\begin{bmatrix}
0 & 1 & … & 1 & 1\\ 
1 & 0 & & & 1\\
\vdots & & \ddots & & \vdots \\
1 & & & 0 & 1\\ 
1 & 1 & … & 1 & 0\\ 
\end{bmatrix}
$$
</p>

Then, with $\delta$ as the Kronecker delta, expected loss is:
<p>
$$
\begin{align*}
E[L_f(Y,f(X))] &= \sum_{k=1}^K (1-\delta_{c_k, f(X)})\mathbb{P}(c_k|X) \\\\
E[L_f(Y,f(X))] &= \sum_{k \neq f(X)}(X)\mathbb{P}(c_k|X) \\\\
E[L_f(Y,f(X))] &= 1−\mathbb{P}(f(X)|X)
\end{align*}
$$
</p>

The optimal choice of f, that minimizes $1−Pr(f(X)|X)$ for every x is therefore:
<p>
$$
\begin{align*}
f(x) &= argmin_{c = c_1 \ldots c_K} \ 1-\mathbb{P}(c|X=x) \\\\
f(x) &= argmax_{c = c_1 \ldots c_K} \ \mathbb{P}(c|X=x)
\end{align*}
$$
</p>

This predictor is known as the Bayes classifier. Since these "true" probabilities are essentially never known thus $\mathbb{P}(c|X=x)$ is very difficult to compute in practice, this is more a theoretical concept and not something that you can actually use. By applying the Bayes theorem for this quantity then assuming mutual independence of all the parameters:
<p>
$$
\begin{align*}
f(x) &= argmax_{c = c_1 \ldots c_K} \ \mathbb{P}(X=x|c) \mathbb{P}(c) \\\\
f(x) &= \prod_{i=1}^K \mathbb{P}(X=x∣c_i)
\end{align*}
$$
</p>

This is the [Naive Bayes classifier](https://en.wikipedia.org/wiki/Naive_Bayes_classifier), which can then be implemented in practice.

### One problem, two solutions

All of the predictors we studied in the previous sections have significant drawbacks: if special structure exists in input data $X$, a regression can't reduce both the bias and the variance of the estimates. Also, a regression can result in large errors if the dimension of $X$ is high.

Our goal is to find a useful approximation $\hat{f}(x)$ to the function $f(x)$ that underlies the predictive and potentially structured relationship between the inputs and outputs with large, high-dimensional, and potentially infinite state spaces. 

This problem can be tackled by two different approaches. 

## Statistical estimators
The approach taken in applied mathematics and statistics has been from the perspective of function approximation and estimation.
Input and output are now viewed as points in an euclidian space, and the prediction estimator is mapping the pairs in a hyperplan of this space. The estimator is now expressed as:

<p>
$$
y_i = \hat{f} (x_i) + \epsilon_i
$$
</p>

Estimators will be chosen by imposing some heavy restrictions on the class of models being fitted: roughness penalty, kernel methods and basis functions are some of them.

## Learning by example
This approach attempts to learn f through a teacher which aims to minimize the loss function. Teacher's goal is to produce outputs $\hat{f}(x_i)$ in response to the inputs. Upon completion of the learning process, we want predicted and real outputs to be close enough to be useful for all sets of inputs likely to be encountered in practice.
This approach is often referred as supervised learning. *Machine learning as we know it was intended to be an answer to this problem*.

### References 

Hastie, Trevor, Tibshirani, Robert and Friedman, Jerome. *The Elements of Statistical Learning*. New York, NY, USA: Springer New York Inc., 2001.

Niels Richard, *Prediction and Classification*, University of Copenhagen, 2006

John L. Weatherwax, David Epstein, *A Solution Manual and Notes for: The Elements of Statistical Learning*, Chapter 2, 2020
