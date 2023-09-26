import React, { type FC } from 'react'
import CountUp from 'react-countup'
import { animated, useSpring } from 'react-spring'

import scale from '../../assets/images/scale.png'
import classes from '../BetCoefficientScale/BetCoefficientScale.module.scss'

export interface BetCoefficientScaleProps {
  isAction?: boolean
  coefficient: number
}

const BetCoefficientScale: FC<BetCoefficientScaleProps> = ({
  isAction = true,
  coefficient = 0
}) => {
  const applicationHeight =
    document.documentElement?.clientHeight !== 0
      ? document.documentElement.clientHeight
      : 0
  const maxHeight = 0.4 * applicationHeight

  const scaleStyles = useSpring({
    from: { transform: "translateY('0px')" },
    to: {
      transform: `${
        'translateY(' + (isAction ? '-' + maxHeight + 'px' : '0px') + ')'
      }`
    },
    config: { mass: 6, duration: 1000 }
  })

  return (
    <>
      <animated.div className={classes.betCoefficient} style={scaleStyles}>
        <CountUp
          start={0}
          end={isAction ? coefficient : 0}
          duration={coefficient}
          decimals={2}
          decimal=","
          prefix={'x'}
        />
      </animated.div>
      <img src={scale} alt="" />
    </>
  )
}

export default BetCoefficientScale
